import React, { useState, useEffect } from 'react';
import { TextField, Button } from '@mui/material';
import axios from 'axios';

function ActieForm({ initialAction, onActionAdded, currentUser }) {
    const [formData, setFormData] = useState({
        fldMid: '',
        fldOmschrijving: '',
        fldMActieVoor: '',
        fldMActieDatum: '',
        fldMActieSoort: '',
        fldMStatus: 'Open',
        werknId: currentUser || 0
    });

    useEffect(() => {
        if (initialAction) {
            setFormData({
                fldMid: initialAction.fldMid || '',
                fldOmschrijving: initialAction.fldOmschrijving || '',
                fldMActieVoor: initialAction.fldMActieVoor || '',
                fldMActieDatum: initialAction.fldMActieDatum ? initialAction.fldMActieDatum.split('T')[0] : '',
                fldMActieSoort: initialAction.fldMActieSoort || '',
                fldMStatus: initialAction.fldMStatus || 'Open',
                werknId: initialAction.werknId || currentUser || 0
            });
        } else {
            setFormData({
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieDatum: '',
                fldMActieSoort: '',
                fldMStatus: 'Open',
                werknId: currentUser || 0
            });
        }
    }, [initialAction, currentUser]);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const actionData = {
            ...formData,
            fldMActieDatum: formData.fldMActieDatum ? `${formData.fldMActieDatum}T00:00:00` : null,
            fldMActieVoor: parseInt(formData.fldMActieVoor) || null,
            werknId: parseInt(formData.werknId) || null
        };
        if (!actionData.fldMid) {
            delete actionData.fldMid;
        }

        console.log('Verzonden data:', actionData);
        try {
            if (actionData.fldMid) {
                const response = await axios.put(`https://localhost:44361/api/memos/${actionData.fldMid}`, actionData);
                console.log('Actie bijgewerkt:', response.data);
            } else {
                const response = await axios.post('https://localhost:44361/api/memos', actionData);
                console.log('Actie toegevoegd:', response.data);
            }
            setFormData({
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieDatum: '',
                fldMActieSoort: '',
                fldMStatus: 'Open',
                werknId: currentUser || 0
            });
            if (onActionAdded) onActionAdded();
        } catch (error) {
            console.error('Fout bij opslaan actie:', error.response ? error.response.data : error.message);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <TextField name="fldOmschrijving" label="Beschrijving" value={formData.fldOmschrijving} onChange={handleInputChange} multiline rows={4} fullWidth margin="normal" />
            <TextField name="fldMActieVoor" label="Toegewezen aan (ID)" value={formData.fldMActieVoor} onChange={handleInputChange} type="number" fullWidth margin="normal" />
            <TextField name="fldMActieDatum" label="Vervaldatum" type="date" value={formData.fldMActieDatum} onChange={handleInputChange} InputLabelProps={{ shrink: true }} fullWidth margin="normal" />
            <TextField name="fldMActieSoort" label="Actie Type" value={formData.fldMActieSoort} onChange={handleInputChange} fullWidth margin="normal" />
            <TextField name="fldMStatus" label="Status" value={formData.fldMStatus} onChange={handleInputChange} fullWidth margin="normal" />
            <Button type="submit" variant="contained" color="primary">
                {formData.fldMid ? 'Opslaan' : 'Actie Toevoegen'}
            </Button>
        </form>
    );
}

export default ActieForm;