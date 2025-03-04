import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';
import axios from 'axios';

function ActieDetail({ action, open = false, onClose }) {
    const [werknemers, setWerknemers] = useState([]);
    const [prioriteiten, setPrioriteiten] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        let isMounted = true;
        const fetchData = async () => {
            setIsLoading(true);
            try {
                const [werknemersResponse, prioriteitenResponse] = await Promise.all([
                    axios.get('https://localhost:44361/api/werknemers'),
                    axios.get('https://localhost:44361/api/priorities'),
                ]);
                if (isMounted) {
                    setWerknemers(werknemersResponse.data || []);
                    setPrioriteiten(prioriteitenResponse.data || []);
                }
            } catch (error) {
                console.error('Fout bij ophalen data in ActieDetail:', error);
            } finally {
                if (isMounted) {
                    setIsLoading(false);
                }
            }
        };
        fetchData();
        return () => { isMounted = false };
    }, []);

    const getWerknemerNaam = (werknId) => {
        if (!werknId || !werknemers.length) return 'Onbekend';
        const werknemer = werknemers.find(w => w.werknId === parseInt(werknId));
        return werknemer ? werknemer.voornaam : 'Onbekend';
    };

    const getPrioriteitOmschrijving = (prioriteitId) => {
        if (!prioriteitId || !prioriteiten.length) return 'Geen';
        const prioriteit = prioriteiten.find(p => p.prioriteit === parseInt(prioriteitId));
        return prioriteit ? prioriteit.omschrijving : 'Geen';
    };

    const formatteerDatum = (datum) => {
        return datum ? datum.split('T')[0] : 'Niet ingesteld';
    };

    const isCompleted = !!action?.fldMActieGereed;

    if (isLoading) {
        return (
            <Dialog open={open} onClose={onClose}>
                <DialogTitle>Laden...</DialogTitle>
                <DialogContent>
                    <Typography>Actie-details worden geladen.</Typography>
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={onClose}
                        variant="contained"
                        color="secondary"
                        sx={{
                            textTransform: 'none',
                            backgroundColor: '#f44336', // Roodachtig, past bij sluiten
                            '&:hover': {
                                backgroundColor: '#d32f2f', // Donkerder bij hover
                            },
                        }}
                    >
                        Sluiten
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }

    if (!action) {
        return null;
    }

    return (
        <Dialog
            open={open}
            onClose={onClose}
            PaperProps={{ style: { backgroundColor: isCompleted ? '#D3D3D3' : 'white' } }}
        >
            <DialogTitle>Actie Details</DialogTitle>
            <DialogContent>
                <Typography
                    variant="body1"
                    gutterBottom
                    style={{ textDecoration: isCompleted ? 'line-through' : 'none' }}
                >
                    <strong>Beschrijving:</strong> {action.fldOmschrijving || 'Geen beschrijving'}
                </Typography>
                <Typography variant="body1" gutterBottom>
                    <strong>Actie voor:</strong> {getWerknemerNaam(action.fldMActieVoor)}
                </Typography>
                <Typography variant="body1" gutterBottom>
                    <strong>Tweede actie voor:</strong> {getWerknemerNaam(action.fldMActieVoor2) || 'Niet ingesteld'}
                </Typography>
                <Typography variant="body1" gutterBottom>
                    <strong>Vervaldatum:</strong> {formatteerDatum(action.fldMActieDatum)}
                </Typography>
                <Typography variant="body1" gutterBottom>
                    <strong>Actie soort:</strong> {action.fldMActieSoort || 'Niet ingesteld'}
                </Typography>
                <Typography variant="body1" gutterBottom>
                    <strong>Prioriteit:</strong> {getPrioriteitOmschrijving(action.fldMPrioriteit)}
                </Typography>
                <Typography
                    variant="body1"
                    gutterBottom
                    style={{ color: isCompleted ? 'green' : 'inherit' }}
                >
                    <strong>Status:</strong>{' '}
                    {isCompleted && action.fldMActieGereed
                        ? `Gereed op ${new Date(action.fldMActieGereed).toLocaleDateString('nl-NL')} ${new Date(action.fldMActieGereed).toLocaleTimeString('nl-NL')}`
                        : 'Open'}
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={onClose}
                    variant="contained"
                    color="secondary"
                    sx={{
                        textTransform: 'none',
                        backgroundColor: '#f44336', // Roodachtig, past bij sluiten
                        '&:hover': {
                            backgroundColor: '#d32f2f', // Donkerder bij hover
                        },
                        padding: '8px 16px', // Extra padding voor een mooiere knop
                        borderRadius: '8px', // Afgeronde hoeken
                    }}
                >
                    Sluiten
                </Button>
            </DialogActions>
        </Dialog>
    );
}

ActieDetail.defaultProps = {
    open: false,
};

export default ActieDetail;