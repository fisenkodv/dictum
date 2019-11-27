import './styles.scss';

import { Box, CircularProgress } from '@material-ui/core';
import Button from '@material-ui/core/Button';
import Container from '@material-ui/core/Container';
import Grid from '@material-ui/core/Grid';
import { makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import React, { useEffect, useState } from 'react';
import { Link as RouteLink } from 'react-router-dom';

import { QuotesApi } from '../../services/api';
import { emptyQuote, Quote } from '../../services/models';

const useStyles = makeStyles(theme => ({
    heroContent: {
        backgroundColor: theme.palette.background.paper,
        padding: theme.spacing(8, 0, 6)
    },
    heroButtons: {
        marginTop: theme.spacing(4)
    }
}));

export const Home: React.FC = () => {
    const [initialized, setInitialized] = useState(false);
    const [loading, setLoading] = useState(false);
    const [quote, setQuote] = useState<Quote>(emptyQuote());

    const fetchQuote = async () => {
        setLoading(true);
        const quote = await QuotesApi.getRandomQuote();
        setLoading(false);
        return quote ? setQuote(quote) : setQuote(emptyQuote());
    };

    useEffect(() => {
        if (!initialized) {
            setInitialized(true);
            fetchQuote();
        }
    }, [initialized]);

    const classes = useStyles();

    return (
        <Box className={classes.heroContent}>
            <Container maxWidth="lg">
                <Box textAlign="center">
                    {loading ? (
                        <CircularProgress />
                    ) : (
                        <Box>
                            <Typography variant="h4" align="center" color="textPrimary" gutterBottom className="quote">
                                {quote.text}
                            </Typography>
                            <Typography variant="subtitle1" align="right" color="textSecondary" paragraph>
                                <RouteLink to={`/author/${quote.author}`}>{quote.author}</RouteLink>
                            </Typography>
                        </Box>
                    )}
                </Box>
                <Box className={classes.heroButtons}>
                    <Grid container spacing={2} justify="center">
                        <Grid item>
                            <Button variant="contained" color="primary" disabled={loading} onClick={() => fetchQuote()}>
                                Get Random
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button variant="outlined" color="primary" disabled={loading}>
                                Copy Link
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Container>
        </Box>
    );
};

export default Home;
