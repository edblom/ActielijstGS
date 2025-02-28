import React from 'react';
import { DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';

function ActieDetail({ action, onClose }) {
    if (!action) return null;

    return (
        <>
            <DialogTitle>Actie Details</DialogTitle>
            <DialogContent>
                <Typography><strong>Beschrijving:</strong> {action.fldOmschrijving}</Typography>
                <Typography><strong>Type:</strong> {action.fldMActieSoort}</Typography>
                <Typography><strong>Toegewezen aan ID:</strong> {action.fldMActieVoor}</Typography>
                <Typography><strong>Status:</strong> {action.fldMStatus}</Typography>
                <Typography><strong>Vervaldatum:</strong> {action.fldMActieDatum ? new Date(action.fldMActieDatum).toLocaleDateString('nl-NL') : 'Geen datum'}</Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="primary">Sluiten</Button>
            </DialogActions>
        </>
    );
}

export default ActieDetail;