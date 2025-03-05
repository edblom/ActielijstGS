// theme.js
import { createTheme } from '@mui/material/styles';

export const theme = createTheme({
    palette: {
        primary: {
            main: '#1976d2',
        },
        secondary: {
            main: '#dc004e',
        },
        background: {
            default: '#f5f5f5',
        },
        appBar: {
            main: '#1a3c5e',
        },
    },
    components: {
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: '#1a3c5e',
                    color: '#ffffff',
                },
            },
        },
        MuiDataGrid: {
            styleOverrides: {
                columnHeaders: {
                    backgroundColor: '#ff0000 !important', // Test met rood
                    color: '#000000 !important', // Zwart voor contrast
                },
            },
        },
    },
});