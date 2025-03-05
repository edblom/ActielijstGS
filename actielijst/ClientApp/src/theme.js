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
                columnHeader: {
                    backgroundColor: 'rgba(0, 0, 0, 0.04)', // Standaard MUI grijs
                },
            },
        },
    },
});