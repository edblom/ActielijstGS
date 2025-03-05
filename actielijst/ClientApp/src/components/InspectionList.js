// ClientApp/src/components/InspectionList.js
import React, { useState, useEffect } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { TextField, Button, Box } from '@mui/material';
import axios from 'axios';

function InspectionList({ inspecteurId }) {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterType, setFilterType] = useState('all');

    // State voor kolomzichtbaarheid en breedtes
    const [columnVisibilityModel, setColumnVisibilityModel] = useState({});
    const [columnWidths, setColumnWidths] = useState({});

    // Originele kolomdefinities (default waarden)
    const defaultColumns = [
        //{ field: 'psid', headerName: 'ID', width: 90 },
        { field: 'project', headerName: 'Project', width: 150 },
        //{ field: 'projectNr', headerName: 'Project Nr', width: 120 },
        { field: 'opdracht', headerName: 'Opdracht Nr', width: 120 },
        { field: 'adres', headerName: 'Adres', width: 200 },
        { field: 'applicateur', headerName: 'Applicateur', width: 120 },
        { field: 'soort', headerName: 'Soort', width: 120 },
        { field: 'omschrijving', headerName: 'Omschrijving', width: 200 },
        { field: 'toegewezen', headerName: 'Toegewezen', width: 100, type: 'boolean' },
        {
            field: 'datumGereed',
            headerName: 'Datum Gereed',
            width: 150,
            valueGetter: (params) => (params ? new Date(params) : null),
            valueFormatter: (params) => (params ? params.toLocaleDateString('nl-NL') : ''),
        },
        { field: 'status', headerName: 'Status', width: 120 },
        {
            field: 'appointmentDateTime',
            headerName: 'Afspraak',
            width: 150,
            valueGetter: (params) => (params ? new Date(params) : null),
            valueFormatter: (params) => (params ? params.toLocaleString('nl-NL') : ''),
        },
    ];

    useEffect(() => {
        if (!inspecteurId) return;

        // Laad opgeslagen instellingen uit localStorage bij mounten
        const savedSettings = localStorage.getItem(`gridSettings_${inspecteurId}`);
        if (savedSettings) {
            const settings = JSON.parse(savedSettings);
            setColumnVisibilityModel(settings.visibility || {});
            setColumnWidths(settings.widths || {});
        }

        const fetchInspections = async () => {
            setLoading(true);
            try {
                const response = await axios.get(
                    `https://localhost:44361/api/upcominginspections?inspecteurId=${inspecteurId}`
                );
                setRows(response.data || []);
            } catch (error) {
                console.error('Fout bij ophalen inspecties:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchInspections();
    }, [inspecteurId]);

    const columns = defaultColumns.map(col => ({
        ...col,
        width: columnWidths[col.field] || col.width, // Gebruik opgeslagen breedte of standaard
    }));

    const filteredRows = rows.filter(row => {
        if (!row || typeof row !== 'object') return false;
        return Object.values(row)
            .join(' ')
            .toLowerCase()
            .includes(searchTerm.toLowerCase()) &&
            (filterType === 'all' || row.status === filterType);
    });

    const handleShowAll = () => {
        setFilterType('all');
        setSearchTerm('');
    };

    // Sla instellingen direct op bij wijziging
    const handleColumnVisibilityChange = (newModel) => {
        setColumnVisibilityModel(newModel);
        saveSettings(newModel, columnWidths); // Sla direct op met de nieuwe model
    };

    const handleColumnWidthChange = (params) => {
        const newWidths = {
            ...columnWidths,
            [params.colDef.field]: params.width,
        };
        setColumnWidths(newWidths);
        saveSettings(columnVisibilityModel, newWidths); // Sla direct op met de nieuwe breedtes
    };

    const saveSettings = (visibility, widths) => {
        const settings = {
            visibility: visibility,
            widths: widths,
        };
        localStorage.setItem(`gridSettings_${inspecteurId}`, JSON.stringify(settings));
        console.log(`Settings saved for ${inspecteurId}:`, settings); // Debug-log
    };

    // Reset-functie naar standaardinstellingen
    const handleResetSettings = () => {
        setColumnVisibilityModel({}); // Reset zichtbaarheid (alle kolommen zichtbaar)
        setColumnWidths({}); // Reset breedtes naar standaard
        localStorage.removeItem(`gridSettings_${inspecteurId}`); // Verwijder opgeslagen instellingen
        console.log(`Settings reset to default for ${inspecteurId}`);
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                height: '100%', // Vul de resterende ruimte van App.js
                overflow: 'hidden', // Geen extra scrollbar
            }}
        >
            <Box
                sx={{
                    p: 2,
                    display: 'flex',
                    alignItems: 'center',
                    flexShrink: 0, // Zoekbalk krimpt niet
                    backgroundColor: '#f5f5f5', // Optioneel: visuele scheiding
                }}
            >
                <TextField
                    label="Zoek naar"
                    variant="outlined"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    sx={{ mr: 1 }}
                />
                <Button
                    variant="contained"
                    onClick={handleShowAll}
                    sx={{ mr: 1 }}
                >
                    Alles tonen
                </Button>
                <Button
                    variant="outlined"
                    onClick={handleResetSettings}
                    color="secondary"
                >
                    Herstel
                </Button>
            </Box>
            <Box
                sx={{
                    flex: 1, // Resterende ruimte
                    overflow: 'hidden', // Geen extra scrollbar
                }}
            >
                <DataGrid
                    rows={filteredRows}
                    columns={columns}
                    loading={loading}
                    getRowId={(row) => row?.psid || Math.random()}
                    disableColumnMenu={false} // Enable column menu for visibility and resizing
                    disableRowSelectionOnClick
                    autoHeight={false}
                    columnVisibilityModel={columnVisibilityModel}
                    onColumnVisibilityModelChange={handleColumnVisibilityChange}
                    onColumnWidthChange={handleColumnWidthChange}
                    sx={{
                        '& .MuiDataGrid-root': {
                            border: 'none',
                        },
                        '& .MuiDataGrid-virtualScroller': {
                            overflowY: 'auto', // Scrollbar in tabel
                            maxHeight: 'calc(100vh - 200px)', // 64px AppBar + 16px App padding boven + 88px zoekbalk + 16px App padding onder + 16px marge
                        },
                        '& .MuiDataGrid-footerContainer': {
                            display: 'none', // Geen paginering
                        },
                    }}
                />
            </Box>
        </Box>
    );
}

export default InspectionList;