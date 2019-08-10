import './styles.scss';

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
    const fetchQuote = async () => {
        const quote = await QuotesApi.getRandomQuote();
        return quote ? setQuote(quote) : setQuote(emptyQuote());
    };
    const [initialized, setInitialized] = useState(false);
    const [quote, setQuote] = useState<Quote>(emptyQuote());

    useEffect(() => {
        if (!initialized) {
            setInitialized(true);
            fetchQuote();
        }
    }, [initialized]);

    const classes = useStyles();

    return (
        <div className={classes.heroContent}>
            <Container maxWidth="lg">
                <Typography variant="h4" align="center" color="textPrimary" gutterBottom className="quote">
                    {quote.text}
                </Typography>
                <Typography variant="subtitle1" align="right" color="textSecondary" paragraph>
                    <RouteLink to={`/author/${quote.author}`}>{quote.author}</RouteLink>
                </Typography>
                <div className={classes.heroButtons}>
                    <Grid container spacing={2} justify="center">
                        <Grid item>
                            <Button variant="contained" color="primary" onClick={() => fetchQuote()}>
                                Get Random
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button variant="outlined" color="primary">
                                Copy Link
                            </Button>
                        </Grid>
                    </Grid>
                </div>
            </Container>
        </div>
    );
};

export default Home;
