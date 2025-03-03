import React, { useState } from 'react';
import { Fab, AppBar, Toolbar, IconButton, Menu, MenuItem, Typography, TextField, ThemeProvider } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import AddIcon from '@mui/icons-material/Add';
import ActieLijst from './components/ActieLijst';
import ActieForm from './components/ActieForm';
import ActieDetail from './components/ActieDetail';
import Login from './components/Login';
import Dialog from '@mui/material/Dialog';
import { theme } from './theme';

function App() {
    const [currentUser, setCurrentUser] = useState(null); // WerknId
    const [userVoornaam, setUserVoornaam] = useState(''); // Voornaam
    const [refreshTrigger, setRefreshTrigger] = useState(0);
    const [selectedAction, setSelectedAction] = useState(null);
    const [openForm, setOpenForm] = useState(false);
    const [listType, setListType] = useState('assigned');
    const [anchorEl, setAnchorEl] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedDetailAction, setSelectedDetailAction] = useState(null);
    const [openDetail, setOpenDetail] = useState(false);

    const handleLogin = (loginData) => {
        setCurrentUser(loginData.werknId);
        setUserVoornaam(loginData.voornaam);
    };

    const handleActionAdded = () => {
        setRefreshTrigger(prev => prev + 1);
        setSelectedAction(null);
        setOpenForm(false);
    };

    const handleEditAction = (action) => {
        setSelectedAction(action);
        setOpenForm(true);
    };

    const handleAddAction = () => {
        setSelectedAction(null);
        setOpenForm(true);
    };

    const handleShowDetail = (action) => {
        setSelectedDetailAction(action);
        setOpenDetail(true);
    };

    const handleCloseDetail = () => {
        setSelectedDetailAction(null);
        setOpenDetail(false);
    };

    const handleMenuOpen = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleListChange = (type) => {
        setListType(type);
        setRefreshTrigger(prev => prev + 1);
        handleMenuClose();
    };

    if (!currentUser) {
        return <Login onLogin={handleLogin} />;
    }

    return (
        <ThemeProvider theme={theme}>
            <div className="App" style={{ padding: '20px', position: 'relative', minHeight: '100vh' }}>
                <AppBar position="static">
                    <Toolbar>
                        <Typography variant="h6" style={{ flexGrow: 1 }}>
                            Memo Lijst (Ingelogd als: {userVoornaam})
                        </Typography>
                        <TextField
                            variant="outlined"
                            size="small"
                            placeholder="Zoek memos..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            sx={{
                                marginRight: '10px',
                                backgroundColor: 'white',
                                borderRadius: '4px',
                                '& .MuiInputBase-root': { height: '32px', padding: '0 8px' },
                                '& .MuiInputBase-input': { padding: '0', fontSize: '14px' },
                                '& .MuiOutlinedInput-notchedOutline': { borderColor: 'white' },
                            }}
                        />
                        <IconButton edge="end" color="inherit" aria-label="menu" onClick={handleMenuOpen}>
                            <MenuIcon />
                        </IconButton>
                        <Menu
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={handleMenuClose}
                            anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
                            transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                        >
                            <MenuItem onClick={() => handleListChange('assigned')}>Memos voor Mij</MenuItem>
                            <MenuItem onClick={() => handleListChange('created')}>Memos van Mij</MenuItem>
                        </Menu>
                    </Toolbar>
                </AppBar>

                <h2>{listType === 'assigned' ? 'Memos voor Mij' : 'Memos van Mij'}</h2>
                <ActieLijst
                    userId={currentUser}
                    refreshTrigger={refreshTrigger}
                    onEditAction={handleEditAction}
                    onShowDetail={handleShowDetail}
                    filterType={listType}
                    searchTerm={searchTerm}
                />

                <Fab
                    color="primary"
                    aria-label="add"
                    onClick={handleAddAction}
                    style={{ position: 'fixed', bottom: '20px', right: '20px' }}
                >
                    <AddIcon />
                </Fab>

                <Dialog open={openForm} onClose={() => setOpenForm(false)}>
                    <div style={{ padding: '20px' }}>
                        <ActieForm
                            initialAction={selectedAction}
                            onActionAdded={handleActionAdded}
                            currentUser={currentUser}
                        />
                    </div>
                </Dialog>

                <Dialog open={openDetail} onClose={handleCloseDetail}>
                    <div style={{ padding: '20px' }}>
                        <ActieDetail action={selectedDetailAction} onClose={handleCloseDetail} />
                    </div>
                </Dialog>
            </div>
        </ThemeProvider>
    );
}

export default App;