import { Box, IconButton } from '@material-ui/core';
import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import InputBase from '@material-ui/core/InputBase';
import { fade, makeStyles } from '@material-ui/core/styles';
import Toolbar from '@material-ui/core/Toolbar';
import LocalLibraryTwoToneIcon from '@material-ui/icons/LocalLibraryTwoTone';
import SearchIcon from '@material-ui/icons/Search';
import React from 'react';
import { RouteConfig } from 'react-router-config';
import { Link as RouterLink, LinkProps as RouterLinkProps } from 'react-router-dom';

const useStyles = makeStyles(theme => ({
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

const CustomLink = React.forwardRef<HTMLAnchorElement, RouterLinkProps>((props, ref) => (
    <RouterLink innerRef={ref} {...props} />
));

const NavigationItems = [
    { to: '/', label: 'Home' },
    { to: 'top', label: 'Top' },
    { to: 'authors', label: 'Authors' },
    { to: 'topics', label: 'Topics' }
];

export const Header: React.FC = ({ route }: RouteConfig) => {
    const classes = useStyles();

    return (
        <div>
            <AppBar position="sticky">
                <Toolbar>
                    <IconButton edge="start" color="inherit" aria-label="menu">
                        <LocalLibraryTwoToneIcon />
                    </IconButton>

                    <Box display="flex" flexGrow="1">
                        {NavigationItems.map(section => (
                            <Button key={section.to} color="inherit" component={CustomLink} to={section.to}>
                                {section.label}
                            </Button>
                        ))}
                    </Box>

                    <Box className={classes.search}>
                        <Box className={classes.searchIcon}>
                            <SearchIcon />
                        </Box>
                        <InputBase
                            placeholder="Search…"
                            classes={{
                                root: classes.inputRoot,
                                input: classes.inputInput
                            }}
                        />
                    </Box>
                    <Button color="inherit">Login</Button>
                </Toolbar>
            </AppBar>
        </div>
    );
};

export default Header;
