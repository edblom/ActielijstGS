import React, { useState, useEffect } from 'react';
import { TextField, Select, MenuItem, Button } from '@mui/material';
import axios from 'axios';

function ActieForm({ initialAction, onActionAdded, currentUser }) {
    const [formData, setFormData] = useState({
        fldMid: '',
        fldOmschrijving: '',
        fldMActieVoor: '',
        fldMActieVoor2: '',
        fldMActieDatum: '',
        fldMActieSoort: '',
        fldMActieGereed: null, // Alleen gereedheid veld
        fldMPrioriteit: '', // Start met lege string
        werknId: currentUser || 0,
    });
    const [workers, setWorkers] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]); // Bevat prioriteit, omschrijving, kleur
    const [actieSoorten, setActieSoorten] = useState([]);
    const [isDataLoaded, setIsDataLoaded] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [workersResponse, prioriteitenResponse, actieSoortenResponse] = await Promise.all([
                    axios.get('https://localhost:44361/api/werknemers'),
                    axios.get('https://localhost:44361/api/priorities'),
                    axios.get('https://localhost:44361/api/actiesoorten/all'),
                ]);
                console.log('Workers response:', workersResponse.data);
                console.log('Prioriteiten response:', prioriteitenResponse.data);
                console.log('ActieSoorten response:', actieSoortenResponse.data);
                const sortedWorkers = workersResponse.data.sort((a, b) => a.voornaam.localeCompare(b.voornaam));
                setWorkers(sortedWorkers || []);
                setPrioriteiten(prioriteitenResponse.data || []); // Verwacht { prioriteit, omschrijving, kleur }
                setActieSoorten(actieSoortenResponse.data || []);
                setIsDataLoaded(true);
            } catch (error) {
                console.error('Fout bij ophalen data:', error.response ? error.response.data : error.message);
            }
        };
        fetchData();
    }, []);

    useEffect(() => {
        if (isDataLoaded && initialAction) {
            const validPrioriteit = prioriteiten.find(p => p.prioriteit === parseInt(initialAction.fldMPrioriteit))?.prioriteit.toString() || '0';
            setFormData(prevState => ({
                ...prevState,
                fldMid: initialAction.fldMid || '',
                fldOmschrijving: initialAction.fldOmschrijving || '',
                fldMActieVoor: initialAction.fldMActieVoor || '',
                fldMActieVoor2: initialAction.fldMActieVoor2 || '',
                fldMActieDatum: initialAction.fldMActieDatum ? initialAction.fldMActieDatum.split('T')[0] : '',
                fldMActieSoort: initialAction.fldMActieSoort || '',
                fldMActieGereed: initialAction.fldMActieGereed || null, // Alleen gereedheid
                fldMPrioriteit: validPrioriteit,
                werknId: initialAction.werknId || currentUser || 0,
            }));
        } else if (isDataLoaded && !initialAction) {
            setFormData(prevState => ({
                ...prevState,
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieVoor2: '',
                fldMActieDatum: '',
                fldMActieSoort: '',
                fldMActieGereed: null,
                fldMPrioriteit: '0',
                werknId: currentUser || 0,
            }));
        }
    }, [isDataLoaded, initialAction, currentUser, prioriteiten]);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({ ...formData, [name]: value });
        console.log(`Changed ${name} to ${value} (type: ${typeof value})`);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const actionData = {
            ...formData,
            fldMActieDatum: formData.fldMActieDatum ? `${formData.fldMActieDatum}T00:00:00` : null,
            fldMActieVoor: parseInt(formData.fldMActieVoor) || null,
            fldMActieVoor2: parseInt(formData.fldMActieVoor2) || null,
            werknId: parseInt(formData.werknId) || null,
            fldMPrioriteit: formData.fldMPrioriteit === '' ? 0 : parseInt(formData.fldMPrioriteit),
        };
        if (!actionData.fldMid) delete actionData.fldMid;

        console.log('Verzonden data:', actionData);
        try {
            if (actionData.fldMid) {
                const response = await axios.put(`https://localhost:44361/api/acties/${actionData.fldMid}`, actionData);
                console.log('Actie bijgewerkt - Server response:', response.data);
            } else {
                const response = await axios.post('https://localhost:44361/api/acties', actionData);
                console.log('Actie toegevoegd - Server response:', response.data);
            }
            setFormData({
                fldMid: '',
                fldOmschrijving: '',
                fldMActieVoor: '',
                fldMActieVoor2: '',
                fldMActieDatum: '',
                fldMActieSoort: '',
                fldMActieGereed: null,
                fldMPrioriteit: '0',
                werknId: currentUser || 0,
            });
            if (onActionAdded) onActionAdded();
        } catch (error) {
            console.error('Fout bij opslaan actie:', error.response ? error.response.data : error.message);
        }
    };

    const handleComplete = async () => {
        if (!formData.fldMid) {
            console.warn('Save action first to enable completion');
            return;
        }
        const newDate = formData.fldMActieGereed ? null : new Date().toISOString();
        const updatedData = { fldMActieGereed: newDate };
        console.log('Sending PATCH data for completion to:', `https://localhost:44361/api/acties/${formData.fldMid}`, 'with data:', updatedData);

        try {
            const response = await axios.patch(`https://localhost:44361/api/acties/${formData.fldMid}`, updatedData, {
                headers: { 'Content-Type': 'application/json', 'accept': 'application/json' },
            });
            console.log('PATCH response:', response.status, response.data);
            setFormData(prevState => ({
                ...prevState,
                fldMActieGereed: newDate,
            }));
            if (onActionAdded) onActionAdded();
        } catch (error) {
            console.error('Fout bij afronden actie:', error.response ? {
                status: error.response.status,
                data: error.response.data,
                headers: error.response.headers,
            } : error.message);
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
                label="Bestemd voor"
                value={formData.fldMActieVoor}
                onChange={handleInputChange}
                displayEmpty
                fullWidth
                margin="normal"
            >
                <MenuItem value="">Kies werknemer</MenuItem>
                {workers.map((worker) => (
                    <MenuItem key={worker.werknId} value={worker.werknId.toString()}>
                        {worker.voornaam}
                    </MenuItem>
                ))}
            </Select>
            <Select
                name="fldMActieVoor2"
                value={formData.fldMActieVoor2}
                onChange={handleInputChange}
                displayEmpty
                fullWidth
                margin="normal"
            >
                <MenuItem value="">Kies tweede werknemer (optioneel)</MenuItem>
                {workers.map((worker) => (
                    <MenuItem key={worker.werknId} value={worker.werknId.toString()}>
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
                label="Soort actie"
                value={formData.fldMActieSoort}
                onChange={handleInputChange}
                displayEmpty
                fullWidth
                margin="normal"
            >
                <MenuItem value="">Kies actiesoort</MenuItem>
                {actieSoorten.map((actieSoort) => (
                    <MenuItem key={actieSoort.id} value={actieSoort.omschrijving}>
                        {actieSoort.omschrijving}
                    </MenuItem>
                ))}
            </Select>
            <Select
                name="fldMPrioriteit"
                value={formData.fldMPrioriteit}
                onChange={handleInputChange}
                displayEmpty
                fullWidth
                margin="normal"
            >
                {prioriteiten.map((prioriteit) => (
                    <MenuItem key={prioriteit.prioriteit} value={prioriteit.prioriteit.toString()}>
                        {prioriteit.omschrijving}
                    </MenuItem>
                ))}
            </Select>
            {formData.fldMid && (
                <div>
                    <Button
                        variant="contained"
                        color={formData.fldMActieGereed ? 'success' : 'primary'}
                        onClick={handleComplete}
                        style={{ marginTop: '16px', marginBottom: '16px' }}
                    >
                        {formData.fldMActieGereed ? 'Actie is gereed' : 'Zet actie op gereed'}
                    </Button>
                    {formData.fldMActieGereed && <p style={{ color: 'green' }}>Gereed op: {new Date(formData.fldMActieGereed).toLocaleString('nl-NL')}</p>}
                </div>
            )}
            <Button type="submit" variant="contained" color="primary" style={{ marginTop: '16px' }}>
                {formData.fldMid ? 'Opslaan' : 'Actie Toevoegen'}
            </Button>
        </form>
    );
}

export default ActieForm;