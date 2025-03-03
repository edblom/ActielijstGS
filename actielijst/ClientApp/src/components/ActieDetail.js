import React, { useState, useEffect } from 'react';
import { TextField, Select, MenuItem, Button, Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import axios from 'axios';

function ActieDetail({ action, open, onClose, onSave }) {
    const [formData, setFormData] = useState(action || {});
    const [werknemers, setWerknemers] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]);

    useEffect(() => {
        if (action) {
            setFormData({
                fldMid: action.fldMid || '',
                fldOmschrijving: action.fldOmschrijving || '',
                fldMActieVoor: action.fldMActieVoor || '',
                fldMActieDatum: action.fldMActieDatum ? action.fldMActieDatum.split('T')[0] : '',
                fldMActieSoort: action.fldMActieSoort || '',
                fldMStatus: action.fldMStatus || 'Open',
                fldMPrioriteit: action.fldMPrioriteit || '0', // Standaard '0'
                werknId: action.werknId || 0
            });
        }
    }, [action]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [werknemersResponse, prioriteitenResponse] = await Promise.all([
                    axios.get('https://localhost:44361/api/werknemers'),
                    axios.get('https://localhost:44361/api/priorities')
                ]);
                console.log('Prioriteiten response in ActieDetail:', prioriteitenResponse.data);
                setWerknemers(werknemersResponse.data);
                setPrioriteiten(prioriteitenResponse.data || []);
            } catch (error) {
                console.error('Fout bij ophalen data in ActieDetail:', error);
            }
        };
        fetchData();
    }, []);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const updatedAction = {
            ...formData,
            fldMActieDatum: formData.fldMActieDatum ? `${formData.fldMActieDatum}T00:00:00` : null,
            fldMActieVoor: parseInt(formData.fldMActieVoor) || null,
            werknId: parseInt(formData.werknId) || null,
            fldMPrioriteit: formData.fldMPrioriteit === '' ? 0 : parseInt(formData.fldMPrioriteit)
        };
        try {
            await axios.put(`https://localhost:44361/api/memos/${updatedAction.fldMid}`, updatedAction);
            if (onSave) onSave(updatedAction);
            onClose();
        } catch (error) {
            console.error('Fout bij opslaan actie:', error.response ? error.response.data : error.message);
        }
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Actie Details</DialogTitle>
            <DialogContent>
                <TextField
                    name="fldOmschrijving"
                    label="Beschrijving"
                    value={formData.fldOmschrijving || ''}
                    onChange={handleInputChange}
                    multiline
                    rows={4}
                    fullWidth
                    margin="normal"
                />
                <Select
                    name="fldMActieVoor"
                    value={formData.fldMActieVoor}
                    onChange={handleInputChange}
                    displayEmpty
                    fullWidth
                    margin="normal"
                >
                    <MenuItem value="">Kies werknemer</MenuItem>
                    {werknemers.map((worker) => (
                        <MenuItem key={worker.werknId} value={worker.werknId.toString()}>
                            {worker.voornaam}
                        </MenuItem>
                    ))}
                </Select>
                <TextField
                    name="fldMActieDatum"
                    label="Vervaldatum"
                    type="date"
                    value={formData.fldMActieDatum || ''}
                    onChange={handleInputChange}
                    InputLabelProps={{ shrink: true }}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    name="fldMActieSoort"
                    label="Actie Soort"
                    value={formData.fldMActieSoort || ''}
                    onChange={handleInputChange}
                    fullWidth
                    margin="normal"
                />
                <Select
                    name="fldMPrioriteit"
                    value={formData.fldMPrioriteit}
                    onChange={handleInputChange}
                    displayEmpty
                    fullWidth
                    margin="normal"
                >
                    <MenuItem value="0">Geen</MenuItem>
                    {prioriteiten.map((prioriteit) => (
                        <MenuItem key={prioriteit.prioriteit} value={prioriteit.prioriteit.toString()}>
                            {prioriteit.omschrijving}
                        </MenuItem>
                    ))}
                </Select>
                <TextField
                    name="fldMStatus"
                    label="Status"
                    value={formData.fldMStatus || ''}
                    onChange={handleInputChange}
                    fullWidth
                    margin="normal"
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="secondary">
                    Annuleren
                </Button>
                <Button type="submit" onClick={handleSubmit} color="primary">
                    Opslaan
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default ActieDetail;