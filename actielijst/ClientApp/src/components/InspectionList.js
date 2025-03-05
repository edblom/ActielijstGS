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

    useEffect(() => {
        if (!inspecteurId) return;

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

    const columns = [
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

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                minHeight: 'calc(100vh - 64px)',
                p: 2,
                overflow: 'hidden',
            }}
        >
            <div style={{ marginBottom: 10 }}>
                <TextField
                    label="Zoek naar"
                    variant="outlined"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    style={{ marginRight: 10 }}
                />
                <Button
                    variant="contained"
                    onClick={handleShowAll}
                    style={{ marginRight: 10 }}
                >
                    Alles tonen
                </Button>
            </div>
            <Box sx={{ flex: 1, display: 'flex' }}>
                <DataGrid
                    rows={filteredRows}
                    columns={columns}
                    loading={loading}
                    getRowId={(row) => row?.psid || Math.random()}
                    initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
                    pageSizeOptions={[5, 10, 20]}
                    checkboxSelection={false}
                    disableRowSelectionOnClick
                    disableVirtualization
                    sx={{
                        flex: 1,
                        '& .MuiDataGrid-root': {
                            border: 'none',
                        },
                        '& .MuiDataGrid-virtualScroller': {
                            overflow: 'hidden',
                        },
                    }}
                />
            </Box>
        </Box>
    );
}

export default InspectionList;