import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import axios from 'axios';

function ActieLijst({ userId, refreshTrigger, onEditAction, onShowDetail, filterType, searchTerm }) {
    const [actions, setActions] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]);
    const [loading, setLoading] = useState(true);

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
                console.log(`Fetching actions from: https://localhost:44361/api/memos/user/${userId}/${filterType}`);
                console.log('Actions API response:', actionsResponse.data);
                console.log('Priorities API response:', prioriteitenResponse.data);
                setActions(actionsResponse.data);
                setPrioriteiten(prioriteitenResponse.data);
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

    const handleShowDetail = (action) => {
        if (onShowDetail) onShowDetail(action);
    };

    const handleDelete = async (id) => {
        try {
            await axios.delete(`https://localhost:44361/api/memos/${id}`);
            console.log('Actie verwijderd:', id);
            const response = await axios.get(`https://localhost:44361/api/memos/user/${userId}/${filterType}`);
            setActions(response.data);
        } catch (error) {
            console.error('Fout bij verwijderen actie:', error);
        }
    };

    const filteredActions = actions.filter(action =>
        (action.fldOmschrijving || '').toLowerCase().includes((searchTerm || '').toLowerCase())
    );

    if (loading) {
        return <div>Laden...</div>;
    }

    return (
        <List>
            {filteredActions.length === 0 ? (
                <ListItem>
                    <ListItemText primary="Geen acties gevonden" />
                </ListItem>
            ) : (
                filteredActions.map((action) => {
                    const prioriteit = prioriteiten.find(p => p.id === action.fldMPrioriteit);
                    const backgroundColor = prioriteit && prioriteit.kleur
                        ? lightenColor(prioriteit.kleur, 0.5)
                        : '#FFFFFF';
                    return (
                        <ListItem
                            key={action.fldMid}
                            style={{ backgroundColor }}
                        >
                            <ListItemText
                                primary={action.fldOmschrijving}
                                secondary={`Type: ${action.fldMActieSoort} - Toegewezen aan ID: ${action.fldMActieVoor} - Status: ${action.fldMStatus} - Vervaldatum: ${action.fldMActieDatum ? new Date(action.fldMActieDatum).toLocaleDateString('nl-NL') : 'Geen datum'}`}
                            />
                            <IconButton onClick={() => handleEdit(action)} color="primary" aria-label="bewerken" style={{ marginRight: '10px' }}>
                                <EditIcon />
                            </IconButton>
                            <IconButton onClick={() => handleShowDetail(action)} color="primary" aria-label="details" style={{ marginRight: '10px' }}>
                                <VisibilityIcon />
                            </IconButton>
                            <IconButton onClick={() => handleDelete(action.fldMid)} color="error" aria-label="verwijderen">
                                <DeleteIcon />
                            </IconButton>
                        </ListItem>
                    );
                })
            )}
        </List>
    );
}

export default ActieLijst;