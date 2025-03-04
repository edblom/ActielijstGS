// theme.js (aanpassen of toevoegen)
import { createTheme } from '@mui/material/styles';

export const theme = createTheme({
    palette: {
        primary: {
            main: '#1976d2', // Blauwe accentkleur
        },
        secondary: {
            main: '#dc004e', // Rode accentkleur voor verwijderacties
        },
        background: {
            default: '#f5f5f5', // Lichte achtergrond voor de app
        },
        appBar: {
            main: '#1a3c5e', // Donkere AppBar voor contrast
        },
    },
    components: {
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: '#1a3c5e', // Donkerblauw
                    color: '#ffffff', // Witte tekst
                },
            },
        },
    },
});