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

    const [columnVisibilityModel, setColumnVisibilityModel] = useState({});
    const [columnWidths, setColumnWidths] = useState({});

    const defaultColumns = [
        { field: 'project', headerName: 'Project', width: 150 },
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
        width: columnWidths[col.field] || col.width,
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

    const handleColumnVisibilityChange = (newModel) => {
        setColumnVisibilityModel(newModel);
        saveSettings(newModel, columnWidths);
    };

    const handleColumnWidthChange = (params) => {
        const newWidths = {
            ...columnWidths,
            [params.colDef.field]: params.width,
        };
        setColumnWidths(newWidths);
        saveSettings(columnVisibilityModel, newWidths);
    };

    const saveSettings = (visibility, widths) => {
        const settings = {
            visibility: visibility,
            widths: widths,
        };
        localStorage.setItem(`gridSettings_${inspecteurId}`, JSON.stringify(settings));
        console.log(`Settings saved for ${inspecteurId}:`, settings);
    };

    const handleResetSettings = () => {
        setColumnVisibilityModel({});
        setColumnWidths({});
        localStorage.removeItem(`gridSettings_${inspecteurId}`);
        console.log(`Settings reset to default for ${inspecteurId}`);
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                height: '100%',
                overflow: 'hidden',
            }}
        >
            <Box
                sx={{
                    p: 2,
                    display: 'flex',
                    alignItems: 'center',
                    flexShrink: 0,
                    backgroundColor: '#f5f5f5',
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
                    flex: 1,
                    overflow: 'hidden',
                }}
            >
                <DataGrid
                    rows={filteredRows}
                    columns={columns}
                    loading={loading}
                    getRowId={(row) => row?.psid || Math.random()}
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
                            overflowY: 'auto',
                            maxHeight: 'calc(100vh - 200px)',
                        },
                        '& .MuiDataGrid-footerContainer': {
                            display: 'none',
                        },
                    }}
                />
            </Box>
        </Box>
    );
}

export default InspectionList;