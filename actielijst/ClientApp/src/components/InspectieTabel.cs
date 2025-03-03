using System.Reflection.Emit;

import React, { useState, useEffect } from 'react';
import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import { TextField, Button } from '@mui/material';
import axios from 'axios';

function InspectieTabel()
{
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [filterType, setFilterType] = useState('all'); // Voor filtering (bijv. "Open", "Alle tonen")

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try
            {
                const response = await axios.get('https://localhost:44361/api/inspecties'); // Pas URL aan
                setRows(response.data || []);
            }
            catch (error)
            {
                console.error('Fout bij ophalen inspecties:', error);
            }
            finally
            {
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    const columns = [
        {
    field: 'projectnr', headerName: 'Projectnr.', aantal m²', width: 150, valueGetter: (params) => `${params.row.projectnr} (${params.row.aantalM2})` },
        {
        field: 'opdrachtnr', headerName: 'Opdracht nr.', cert', width: 150 },
        { field: 'locatie', headerName: 'Locatie', width: 200 },
        { field: 'applicateur', headerName: 'Applicateur', width: 150 },
        { field: 'invoerdatum', headerName: 'Invoerdatum', width: 120, type: 'date', valueGetter: (params) => new Date(params.row.invoerdatum) },
        { field: 'keuringSrn', headerName: 'KeuringSrn', width: 120 },
        { field: 'status', headerName: 'Status', width: 120 },
        { field: 'uitvoerdatum', headerName: 'Uitvoerdatum', width: 120, type: 'date', valueGetter: (params) => new Date(params.row.uitvoerdatum) || null },
        { field: 'opmerkingen', headerName: 'Opmerkingen', width: 200 },
        { field: 'pl1', headerName: 'PL1', width: 80 },
        { field: 'pl2', headerName: 'PL2', width: 80 },
        { field: 'keuringnr', headerName: 'KeuringNr', width: 120 },
    ];

            const filteredRows = rows.filter(row =>
                Object.values(row)
                    .join(' ')
                    .toLowerCase()
                    .includes(searchTerm.toLowerCase()) &&
                (filterType === 'all' || row.status === filterType)
            );

            return (
        
                < div style ={ { height: 600, width: '100%' } }>
        
                    < div style ={ { marginBottom: 10 } }>
        
                        < TextField
                    label = "Zoek naar"
                    variant = "outlined"
                    value ={ searchTerm}
            onChange ={ (e) => setSearchTerm(e.target.value)}
            style ={ { marginRight: 10 } }
                />
                < Button
                    variant = "contained"
                    onClick ={ () => setFilterType('all')}
            style ={ { marginRight: 10 } }
                >
                    Alle tonen
                </ Button >
                < Button
                    variant = "contained"
                    onClick ={ () => setFilterType('Open')}
            style ={ { marginRight: 10 } }
                >
                    Open
                </ Button >
            </ div >
            < DataGrid
                rows ={ filteredRows}
            columns ={ columns}
            loading ={ loading}
            pageSize ={ 10}
            rowsPerPageOptions ={ [5, 10, 20]}
            checkboxSelection
            disableSelectionOnClick
                components ={ { Toolbar: GridToolbar } }
            getRowId ={ (row) => row.id || row.projectnr} // Gebruik een unieke ID
            />
        </ div >
    );
        }

        export default InspectieTabel;