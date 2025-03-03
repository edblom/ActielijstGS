import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, IconButton, Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import axios from 'axios';
import ActieDetail from './ActieDetail';

function ActieLijst({ userId, refreshTrigger, onEditAction, filterType, searchTerm }) {
    const [actions, setActions] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedAction, setSelectedAction] = useState(null);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false); // Nieuwe state voor verwijderdialoog
    const [actionToDelete, setActionToDelete] = useState(null); // Actie die verwijderd moet worden

    const lightenColor = (hex, percent) => {
        let r = parseInt(hex.slice(1, 3), 16);
        let g = parseInt(hex.slice(3, 5), 16);
        let b = parseInt(hex.slice(5, 7), 16);

        r = Math.min(255, Math.round(r + (255 - r) * percent));
        g = Math.min(255, Math.round(g + (255 - g) * percent));
        b = Math.min(255, Math.round(b + (255 - b) * percent));

        return `#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`;
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [actionsResponse, prioriteitenResponse] = await Promise.all([
                    axios.get(`https://localhost:44361/api/memos/user/${userId}/${filterType}`),
                    axios.get('https://localhost:44361/api/priorities')
                ]);
                console.log('Prioriteiten response in ActieLijst:', prioriteitenResponse.data);
                setActions(actionsResponse.data);
                setPrioriteiten(prioriteitenResponse.data || []);
            } catch (error) {
                console.error('Fout bij ophalen acties of prioriteiten:', error.response ? error.response.data : error.message);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [userId, refreshTrigger, filterType]);

    const handleEdit = (action) => {
        if (onEditAction) onEditAction(action);
    };

    const handleOpenDetail = (action) => {
        setSelectedAction(action);
    };

    const handleCloseDetail = () => {
        setSelectedAction(null);
    };

    const handleDeleteConfirm = async () => {
        if (actionToDelete) {
            try {
                await axios.delete(`https://localhost:44361/api/memos/${actionToDelete.fldMid}`);
                console.log('Actie verwijderd:', actionToDelete.fldMid);
                const response = await axios.get(`https://localhost:44361/api/memos/user/${userId}/${filterType}`);
                setActions(response.data);
            } catch (error) {
                console.error('Fout bij verwijderen actie:', error);
            } finally {
                setDeleteDialogOpen(false);
                setActionToDelete(null);
            }
        }
    };

    const handleDeleteOpen = (action) => {
        setActionToDelete(action);
        setDeleteDialogOpen(true);
    };

    const handleDeleteClose = () => {
        setDeleteDialogOpen(false);
        setActionToDelete(null);
    };

    const handleSaveDetail = (updatedAction) => {
        setActions(actions.map(action => action.fldMid === updatedAction.fldMid ? updatedAction : action));
        setSelectedAction(null);
    };

    const filteredActions = actions.filter(action =>
        (action.fldOmschrijving || '').toLowerCase().includes((searchTerm || '').toLowerCase())
    );

    if (loading) {
        return <div>Laden...</div>;
    }

    return (
        <>
            <List>
                {filteredActions.length === 0 ? (
                    <ListItem>
                        <ListItemText primary="Geen acties gevonden" />
                    </ListItem>
                ) : (
                    filteredActions.map((action) => {
                        const prioriteit = prioriteiten.find(p => p.prioriteit === action.fldMPrioriteit);
                        const backgroundColor = prioriteit && prioriteit.kleur
                            ? lightenColor(prioriteit.kleur, 0.2)
                            : '#F0F0F0';
                        return (
                            <ListItem
                                key={action.fldMid}
                                style={{ backgroundColor, cursor: 'pointer' }}
                                onClick={() => handleOpenDetail(action)}
                            >
                                <ListItemText
                                    primary={action.fldOmschrijving}
                                    secondary={`Type: ${action.fldMActieSoort} - Toegewezen aan ID: ${action.fldMActieVoor} - Status: ${action.fldMStatus} - Vervaldatum: ${action.fldMActieDatum ? new Date(action.fldMActieDatum).toLocaleDateString('nl-NL') : 'Geen datum'}`}
                                />
                                <IconButton onClick={(e) => { e.stopPropagation(); handleEdit(action); }} color="primary" aria-label="bewerken" style={{ marginRight: '10px' }}>
                                    <EditIcon />
                                </IconButton>
                                <IconButton onClick={(e) => { e.stopPropagation(); handleDeleteOpen(action); }} color="error" aria-label="verwijderen">
                                    <DeleteIcon />
                                </IconButton>
                            </ListItem>
                        );
                    })
                )}
            </List>
            <ActieDetail
                action={selectedAction}
                open={!!selectedAction}
                onClose={handleCloseDetail}
                onSave={handleSaveDetail}
            />
            {/* Verwijderdialoog */}
            <Dialog open={deleteDialogOpen} onClose={handleDeleteClose}>
                <DialogTitle>Bevestig verwijdering</DialogTitle>
                <DialogContent>
                    <Typography>Weet je zeker dat je de actie "<strong>{actionToDelete?.fldOmschrijving || 'Onbekende actie'}</strong>" wilt verwijderen?</Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleDeleteClose} color="primary">
                        Annuleren
                    </Button>
                    <Button onClick={handleDeleteConfirm} color="error">
                        Verwijderen
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default ActieLijst;