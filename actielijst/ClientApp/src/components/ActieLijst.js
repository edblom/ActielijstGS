import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, IconButton, Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography, Select, MenuItem, FormControl } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import CancelIcon from '@mui/icons-material/Cancel';
import axios from 'axios';
import ActieDetail from './ActieDetail';

function ActieLijst({ userId, refreshTrigger, onEditAction, filterType, searchTerm }) {
    const [actions, setActions] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]);
    const [workers, setWorkers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedAction, setSelectedAction] = useState(null);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [actionToDelete, setActionToDelete] = useState(null);
    const [statusFilter, setStatusFilter] = useState('open');
    const [priorityFilter, setPriorityFilter] = useState('all');

    const lightenColor = (hex, percent) => {
        let r = parseInt(hex.slice(1, 3), 16);
        let g = parseInt(hex.slice(3, 5), 16);
        let b = parseInt(hex.slice(5, 7), 16);

        r = Math.min(255, Math.round(r + (255 - r) * percent));
        g = Math.min(255, Math.round(g + (255 - g) * percent));
        b = Math.min(255, Math.round(b + (255 - b) * percent));

        return `#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`;
    };

    const getLighterGray = (percent) => {
        const baseGray = parseInt('D3', 16);
        const lighterGray = Math.min(255, Math.round(baseGray + (255 - baseGray) * percent));
        return `#${lighterGray.toString(16).padStart(2, '0')}${lighterGray.toString(16).padStart(2, '0')}${lighterGray.toString(16).padStart(2, '0')}`;
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [actionsResponse, prioriteitenResponse, workersResponse] = await Promise.all([
                    axios.get(`https://localhost:44361/api/acties/user/${userId}/${filterType}`),
                    axios.get('https://localhost:44361/api/priorities'),
                    axios.get('https://localhost:44361/api/werknemers'),
                ]);
                console.log('Prioriteiten response in ActieLijst:', prioriteitenResponse.data);
                console.log('Workers response in ActieLijst:', workersResponse.data);
                setActions(actionsResponse.data);
                setPrioriteiten(prioriteitenResponse.data || []);
                setWorkers(workersResponse.data.sort((a, b) => a.voornaam.localeCompare(b.voornaam)) || []);
            } catch (error) {
                console.error('Fout bij ophalen acties, prioriteiten of workers:', error.response ? error.response.data : error.message);
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
                await axios.delete(`https://localhost:44361/api/acties/${actionToDelete.fldMid}`);
                console.log('Actie verwijderd:', actionToDelete.fldMid);
                const response = await axios.get(`https://localhost:44361/api/acties/user/${userId}/${filterType}`);
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

    const getVoornaam = (werknId) => {
        const worker = workers.find(w => w.werknId === parseInt(werknId));
        return worker ? worker.voornaam : 'Onbekend';
    };

    const filteredActions = actions
        .filter(action => {
            const matchesSearch = (action.fldOmschrijving || '').toLowerCase().includes(searchTerm.toLowerCase());
            const matchesStatus = statusFilter === 'all' ||
                (statusFilter === 'open' && !action.fldMActieGereed) ||
                (statusFilter === 'gereed' && action.fldMActieGereed);
            const matchesPriority = priorityFilter === 'all' ||
                action.fldMPrioriteit === parseInt(priorityFilter);
            return matchesSearch && matchesStatus && matchesPriority;
        })
        .sort((a, b) => {
            const dateA = new Date(a.fldMActieDatum || '9999-12-31'); // Default voor geen datum
            const dateB = new Date(b.fldMActieDatum || '9999-12-31');
            return dateA - dateB; // Aflopend: nieuwste datum eerst
        });

    if (loading) {
        return <div>Laden...</div>;
    }

    return (
        <>
            <div style={{ display: 'flex', gap: '16px', marginBottom: '16px', alignItems: 'center' }}>
                <span>Status:</span>
                <FormControl style={{ minWidth: '120px' }}>
                    <Select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} displayEmpty>
                        <MenuItem value="all">Alle</MenuItem>
                        <MenuItem value="open">Open</MenuItem>
                        <MenuItem value="gereed">Gereed</MenuItem>
                    </Select>
                </FormControl>
                <span>Prioriteit:</span>
                <FormControl style={{ minWidth: '120px' }}>
                    <Select value={priorityFilter} onChange={(e) => setPriorityFilter(e.target.value)} displayEmpty>
                        <MenuItem value="all">Alle</MenuItem>
                        {prioriteiten.map((prioriteit) => (
                            <MenuItem key={prioriteit.prioriteit} value={prioriteit.prioriteit.toString()}>
                                {prioriteit.omschrijving}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </div>

            <List
                style={{
                    maxHeight: '70vh', // Maximale hoogte gebaseerd op 70% van de viewport-hoogte
                    overflowY: 'auto', // Voeg verticale scrollbar toe
                    padding: '10px',
                    backgroundColor: '#f5f5f5', // Optioneel: achtergrondkleur voor duidelijkheid
                }}
            >
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
                        const isCompleted = !!action.fldMActieGereed;
                        const voornaam1 = getVoornaam(action.fldMActieVoor);
                        const voornaam2 = action.fldMActieVoor2 ? getVoornaam(action.fldMActieVoor2) : null;
                        const assignedToText = voornaam2 ? `Voor ${voornaam1} / ${voornaam2}` : `Voor ${voornaam1}`;
                        return (
                            <ListItem
                                key={action.fldMid}
                                style={{
                                    backgroundColor: isCompleted ? '#D3D3D3' : backgroundColor,
                                    cursor: 'pointer',
                                    transition: 'background-color 0.2s',
                                    marginBottom: '8px',
                                    borderRadius: '5px',
                                }}
                                onMouseEnter={(e) => {
                                    if (isCompleted) {
                                        e.currentTarget.style.backgroundColor = getLighterGray(0.3);
                                    } else {
                                        e.currentTarget.style.backgroundColor = lightenColor(backgroundColor.replace('#', ''), 0.3);
                                    }
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.backgroundColor = isCompleted ? '#D3D3D3' : backgroundColor;
                                }}
                                onClick={() => handleOpenDetail(action)}
                            >
                                <ListItemText
                                    primary={action.fldOmschrijving}
                                    secondary={`Type: ${action.fldMActieSoort} - ${assignedToText} - Status: ${action.fldMActieGereed ? 'Gereed' : 'Open'} - Vervaldatum: ${action.fldMActieDatum ? new Date(action.fldMActieDatum).toLocaleDateString('nl-NL') : 'Geen datum'}`}
                                    primaryTypographyProps={{
                                        style: {
                                            textDecoration: isCompleted ? 'line-through' : 'none',
                                        },
                                    }}
                                />
                                <IconButton onClick={(e) => { e.stopPropagation(); handleEdit(action); }} color="primary" aria-label="bewerken" style={{ marginRight: '10px', boxShadow: '0 2px 4px rgba(0,0,0,0.1)' }}>
                                    <EditIcon />
                                </IconButton>
                                <IconButton onClick={(e) => { e.stopPropagation(); handleDeleteOpen(action); }} color="error" aria-label="verwijderen" style={{ boxShadow: '0 2px 4px rgba(0,0,0,0.1)' }}>
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
            <Dialog open={deleteDialogOpen} onClose={handleDeleteClose} PaperProps={{ style: { backgroundColor: '#ffffff', padding: '16px', borderRadius: '8px' } }}>
                <DialogTitle>Bevestig verwijdering</DialogTitle>
                <DialogContent>
                    <Typography>Weet je zeker dat je de actie "<strong>{actionToDelete?.fldOmschrijving || 'Onbekende actie'}</strong>" wilt verwijderen?</Typography>
                </DialogContent>
                <DialogActions>
                    <IconButton onClick={handleDeleteClose} color="primary" aria-label="annuleer">
                        <CancelIcon />
                    </IconButton>
                    <IconButton onClick={handleDeleteConfirm} color="error" aria-label="verwijderen">
                        <DeleteIcon />
                    </IconButton>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default ActieLijst;