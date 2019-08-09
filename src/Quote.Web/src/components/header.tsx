import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import InputBase from '@material-ui/core/InputBase';
import Link from '@material-ui/core/Link';
import { fade, makeStyles } from '@material-ui/core/styles';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import SearchIcon from '@material-ui/icons/Search';
import CameraIcon from '@material-ui/icons/SupervisedUserCircleOutlined';
import React from 'react';
import { RouteConfig } from 'react-router-config';

const useStyles = makeStyles(theme => ({
    title: {
        flexGrow: 1
    },
    icon: {
        marginRight: theme.spacing(1)
    },

    toolbar: {
        borderBottom: `1px solid ${theme.palette.divider}`
    },
    toolbarTitle: {
        flex: 1
    },
    toolbarSecondary: {
        justifyContent: 'space-evenly',
        overflowX: 'auto'
    },
    toolbarLink: {
        padding: theme.spacing(1),
        flexShrink: 0
    },

    search: {
        position: 'relative',
        borderRadius: theme.shape.borderRadius,
        backgroundColor: fade(theme.palette.common.white, 0.15),
        '&:hover': {
            backgroundColor: fade(theme.palette.common.white, 0.25)
        },
        marginLeft: 0,
        width: '100%',
        [theme.breakpoints.up('sm')]: {
            marginLeft: theme.spacing(1),
            width: 'auto'
        }
    },
    searchIcon: {
        width: theme.spacing(7),
        height: '100%',
        position: 'absolute',
        pointerEvents: 'none',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
    },
    inputRoot: {
        color: 'inherit'
    },
    inputInput: {
        padding: theme.spacing(1, 1, 1, 7),
        transition: theme.transitions.create('width'),
        width: '100%',
        [theme.breakpoints.up('sm')]: {
            width: 120,
            '&:focus': {
                width: 200
            }
        }
    }
}));

export const Header: React.FC = ({ route }: RouteConfig) => {
    const classes = useStyles();

    const sections = ['Home', 'Top', 'Authors', 'Topics'];
    return (
        <AppBar position="relative">
            <Toolbar>
                <CameraIcon className={classes.icon} />
                <Typography variant="h6" color="inherit" noWrap className={classes.title}>
                    Most Inspiring Expressions Of Mankind
                </Typography>
                <div className={classes.search}>
                    <div className={classes.searchIcon}>
                        <SearchIcon />
                    </div>
                    <InputBase
                        placeholder="Search…"
                        classes={{
                            root: classes.inputRoot,
                            input: classes.inputInput
                        }}
                        inputProps={{ 'aria-label': 'search' }}
                    />
                </div>
                <Button color="inherit">Login</Button>
            </Toolbar>
            {/* <Toolbar component="nav" variant="dense" disableGutters className={classes.toolbarSecondary}>
                {sections.map(section => (
                    <Link color="inherit" noWrap key={section} variant="body2" href="#" className={classes.toolbarLink}>
                        {section}
                    </Link>
                ))}
            </Toolbar> */}
        </AppBar>
    );
};

export default Header;
