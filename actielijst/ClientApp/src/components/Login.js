import React, { useState } from 'react';
import axios from 'axios';
import { TextField, Button, Box, Typography, Alert } from '@mui/material';

function Login({ onLogin }) {
    const [voornaam, setVoornaam] = useState('');
    const [fldLoginNaam, setFldLoginNaam] = useState('');
    const [error, setError] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('https://localhost:44361/api/login', {
                Voornaam: voornaam,
                FldLoginNaam: fldLoginNaam
            });
            console.log('Login response:', response.data);
            onLogin(response.data); // Geeft object met werknId en voornaam door
            setError(null);
        } catch (err) {
            console.error('Login fout:', err.response ? err.response.data : err.message);
            setError('Login mislukt. Controleer je gegevens.');
        }
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                minHeight: '100vh',
                padding: 2,
                backgroundColor: '#f5f5f5',
            }}
        >
            <Box
                sx={{
                    width: '100%',
                    maxWidth: 400,
                    backgroundColor: 'white',
                    padding: 3,
                    borderRadius: 2,
                    boxShadow: 3,
                }}
            >
                <Typography variant="h5" align="center" gutterBottom>
                    Inloggen
                </Typography>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                <form onSubmit={handleSubmit}>
                    <TextField
                        label="Voornaam"
                        variant="outlined"
                        fullWidth
                        margin="normal"
                        value={voornaam}
                        onChange={(e) => setVoornaam(e.target.value)}
                    />
                    <TextField
                        label="Login Naam"
                        type="password"
                        variant="outlined"
                        fullWidth
                        margin="normal"
                        value={fldLoginNaam}
                        onChange={(e) => setFldLoginNaam(e.target.value)}
                    />
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        fullWidth
                        sx={{ mt: 2 }}
                    >
                        Inloggen
                    </Button>
                </form>
            </Box>
        </Box>
    );
}

export default Login;