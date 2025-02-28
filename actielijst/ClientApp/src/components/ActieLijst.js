import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import axios from 'axios';

function ActieLijst({ userId, refreshTrigger, onEditAction, onShowDetail, filterType, searchTerm }) {
    const [actions, setActions] = useState([]);

    useEffect(() => {
        const fetchActions = async () => {
            try {
                const url = `https://localhost:44361/api/memos/user/${userId}/${filterType}`;
                console.log(`Fetching actions from: ${url}`);
                const response = await axios.get(url);
                console.log('API response:', response.data);
                setActions(response.data);
            } catch (error) {
                console.error('Fout bij ophalen acties:', error.response ? error.response.data : error.message);
            }
        };
        fetchActions();
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

    return (
        <List>
            {filteredActions.length === 0 ? (
                <ListItem>
                    <ListItemText primary="Geen acties gevonden" />
                </ListItem>
            ) : (
                filteredActions.map((action) => (
                    <ListItem key={action.fldMid}>
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
                ))
            )}
        </List>
    );
}

export default ActieLijst;