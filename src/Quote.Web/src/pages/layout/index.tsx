import { Box, NoSsr } from '@material-ui/core';
import CssBaseline from '@material-ui/core/CssBaseline';
import { createMuiTheme, makeStyles, ThemeProvider } from '@material-ui/core/styles';
import React from 'react';
import { renderRoutes, RouteConfig } from 'react-router-config';

import Footer from '../../components/footer';
import Header from '../../components/header';

const useStyles = makeStyles(theme => ({
    root: {
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh'
    }
}));

const theme = createMuiTheme();

export const Layout: React.FC = ({ route }: RouteConfig) => {
    const classes = useStyles();

    return (
        <NoSsr>
            <ThemeProvider theme={theme}>
                <Box className={classes.root}>
                    <CssBaseline />
                    <Header></Header>
                    <main>{renderRoutes(route.routes)}</main>
                    <Footer></Footer>
                </Box>
            </ThemeProvider>
        </NoSsr>
    );
};

export default Layout;
