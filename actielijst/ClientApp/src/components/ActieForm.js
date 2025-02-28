import React, { useState, useEffect } from 'react';
import { TextField, Select, MenuItem, Button } from '@mui/material';
import axios from 'axios';

function ActieForm({ initialAction, onActionAdded, currentUser }) {
    const [formData, setFormData] = useState({
        fldMid: '',
        fldOmschrijving: '',
        fldMActieVoor: '',
        fldMActieDatum: '',
        fldMActieSoort: '',
        werknId: currentUser || 0
    });
    const [workers, setWorkers] = useState([]);
    const [actionTypes, setActionTypes] = useState([]);
    const [loadingActionTypes, setLoadingActionTypes] = useState(true);

    // Initialiseer formData bij laden of bij verandering van initialAction/currentUser
    useEffect(() => {
        if (initialAction) {
            setFormData({
                fldMid: initialAction.fldMid || '',
                fldOmschrijving: initialAction.fldOmschrijving || '',
                fldMActieVoor: initialAction.fldMActieVoor || '',
                fldMActieDatum: initialAction.fldMActieDatum ? initialAction.fldMActieDatum.split('T')[0] : '',
                fldMActieSoort: initialAction.fldMActieSoort || '',
                werknId: initialAction.werknId || currentUser || 0
            });
        } else {
            setFormData({
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieDatum: '',
                fldMActieSoort: '',
                werknId: currentUser || 0
            });
        }
    }, [initialAction, currentUser]);

    // Haal werknemers uit de API
    useEffect(() => {
        axios.get('https://localhost:44361/api/werknemers')
            .then(response => {
                const sortedWorkers = response.data.sort((a, b) =>
                    a.voornaam.localeCompare(b.voornaam)
                );
                setWorkers(sortedWorkers);
            })
            .catch(error => console.error('Fout bij ophalen werknemers:', error));
    }, []);

    // Haal actiesoorten uit de API
    useEffect(() => {
        axios.get('https://localhost:44361/api/actiesoorten/all')
            .then(response => {
                setActionTypes(response.data);
                if (!initialAction && response.data.length > 0) {
                    setFormData(prev => ({ ...prev, fldMActieSoort: response.data[0].omschrijving }));
                }
                setLoadingActionTypes(false);
            })
            .catch(error => {
                console.error('Fout bij ophalen actiesoorten:', error);
                setLoadingActionTypes(false);
            });
    }, [initialAction]);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        // Voor fldMActieSoort zoeken we de omschrijving op basis van het geselecteerde ID
        if (name === 'fldMActieSoort') {
            const selectedType = actionTypes.find(type => type.id === parseInt(value));
            setFormData({ ...formData, [name]: selectedType ? selectedType.omschrijving : '' });
        } else {
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const actionData = {
            ...formData,
            fldMActieDatum: formData.fldMActieDatum ? `${formData.fldMActieDatum}T00:00:00` : null,
            fldMActieVoor: parseInt(formData.fldMActieVoor) || null,
            werknId: parseInt(formData.werknId) || null,
            fldMActieSoort: formData.fldMActieSoort || null // Dit is nu de omschrijving
        };
        if (!actionData.fldMid) {
            delete actionData.fldMid;
        }

        console.log('Verzonden data:', JSON.stringify(actionData, null, 2));
        try {
            let response;
            if (actionData.fldMid) {
                response = await axios.put(`https://localhost:44361/api/memos/${actionData.fldMid}`, actionData);
                console.log('Actie bijgewerkt:', response.data);
            } else {
                response = await axios.post('https://localhost:44361/api/memos', actionData);
                console.log('Actie toegevoegd:', response.data);
            }
            setFormData({
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieDatum: '',
                fldMActieSoort: actionTypes.length > 0 ? actionTypes[0].omschrijving : '',
                werknId: currentUser || 0
            });
            if (onActionAdded) onActionAdded();
        } catch (error) {
            console.error('Fout bij opslaan actie:', {
                message: error.message,
                response: error.response ? error.response.data : null,
                status: error.response ? error.response.status : null
            });
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <TextField
                name="fldOmschrijving"
                label="Beschrijving"
                value={formData.fldOmschrijving}
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
                {workers.map((worker) => (
                    <MenuItem key={worker.werknId} value={worker.werknId}>
                        {worker.voornaam}
                    </MenuItem>
                ))}
            </Select>
            <TextField
                name="fldMActieDatum"
                label="Vervaldatum"
                type="date"
                value={formData.fldMActieDatum}
                onChange={handleInputChange}
                InputLabelProps={{ shrink: true }}
                fullWidth
                margin="normal"
            />
            <Select
                name="fldMActieSoort"
                value={actionTypes.find(type => type.omschrijving === formData.fldMActieSoort)?.id || ''}
                onChange={handleInputChange}
                displayEmpty
                fullWidth
                margin="normal"
                disabled={loadingActionTypes}
            >
                <MenuItem value="">Kies actiesoort</MenuItem>
                {actionTypes.map((type) => (
                    <MenuItem key={type.id} value={type.id}>
                        {type.omschrijving}
                    </MenuItem>
                ))}
            </Select>
            <Button type="submit" variant="contained" color="primary" disabled={loadingActionTypes}>
                {formData.fldMid ? 'Opslaan' : 'Actie Toevoegen'}
            </Button>
        </form>
    );
}

export default ActieForm;