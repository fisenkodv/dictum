import { makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import React from 'react';
import { RouteConfig } from 'react-router-config';

const useStyles = makeStyles(theme => ({
    footer: {
        backgroundColor: theme.palette.background.paper,
        marginTop: 'auto',
        padding: theme.spacing(2)
    }
}));

export const Footer: React.FC = ({ route }: RouteConfig) => {
    const classes = useStyles();

    return (
        <footer className={classes.footer}>
            <Typography variant="subtitle1" align="center" color="textSecondary" component="p">
                Dictum - Most Inspiring Expressions Of Mankind
            </Typography>
        </footer>
    );
};

export default Footer;
