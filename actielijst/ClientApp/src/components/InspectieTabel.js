import React, { useState, useEffect } from 'react';
import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import { TextField, Button, Box } from '@mui/material';
import axios from 'axios';

function InspectieTabel() {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterType, setFilterType] = useState('all');
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(100);
    const [total, setTotal] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await axios.get('https://localhost:44361/api/inspecties', {
                    params: { page, pageSize }
                });
                setRows(response.data.Data);
                setTotal(response.data.Total);
            } catch (error) {
                console.error('Fout bij ophalen inspecties:', error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [page, pageSize]);

    const handlePageChange = (params) => {
        setPage(params.page + 1); // DataGrid gebruikt 0-based index
    };

    const handlePageSizeChange = (newPageSize) => {
        setPage(1); // Reset naar eerste pagina bij wijzigen pagina-grootte
        setPageSize(newPageSize);
    };

    const columns = [
        { field: 'projectnr', headerName: 'Projectnr., aantal m²', width: 150, valueGetter: (params) => `${params.row.projectnr} (${params.row.aantalM2})` },
        { field: 'opdrachtnr', headerName: 'Opdracht nr., cert', width: 150 },
        { field: 'locatie', headerName: 'Locatie', width: 200 },
        { field: 'applicateur', headerName: 'Applicateur', width: 150 },
        { field: 'invoerdatum', headerName: 'Invoerdatum', width: 120, type: 'date', valueGetter: (params) => new Date(params.row.invoerdatum) },
        { field: 'keuringSrn', headerName: 'KeuringSrn', width: 120 },
        { field: 'status', headerName: 'Status', width: 120 },
        { field: 'uitvoerdatum', headerName: 'Uitvoerdatum', width: 120, type: 'date', valueGetter: (params) => new Date(params.row.uitvoerdatum) || null },
        { field: 'opmerkingen', headerName: 'Opmerkingen', width: 200 },
    ];

    const filteredRows = rows.filter(row =>
        Object.values(row)
            .join(' ')
            .toLowerCase()
            .includes(searchTerm.toLowerCase()) &&
        (filterType === 'all' || row.status === filterType)
    );

    return (
        <div>
            <h2>KIWA Inspecties</h2>
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
                    onClick={() => setFilterType('all')}
                    style={{ marginRight: 10 }}
                >
                    Alle tonen
                </Button>
                <Button
                    variant="contained"
                    onClick={() => setFilterType('Open')}
                    style={{ marginRight: 10 }}
                >
                    Open
                </Button>
            </div>
            <DataGrid
                rows={filteredRows}
                columns={columns}
                loading={loading}
                pageSize={pageSize}
                rowCount={total}
                page={page - 1} // DataGrid gebruikt 0-based index
                onPageChange={handlePageChange}
                onPageSizeChange={handlePageSizeChange}
                pagination
                paginationMode="server"
                components={{ Toolbar: GridToolbar }}
                getRowId={(row) => row.id || row.projectnr}
                style={{ height: 600, width: '100%' }}
            />
        </div>
    );
}

export default InspectieTabel;