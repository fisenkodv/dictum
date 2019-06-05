import './styles.scss';

import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

import { QuotesApi } from '../../services/api';
import { Quote } from '../../services/models';

export const Home: React.FC = () => {
    const fetchQuote = async () => {
        const quote = await QuotesApi.getRandomQuote();
        return setQuote(quote);
    };
    const [initialized, setInitialized] = useState(false);
    const [quote, setQuote] = useState<Quote>({ uuid: '', author: '', text: '' });

    useEffect(() => {
        if (!initialized) {
            setInitialized(true);
            fetchQuote();
        }
    }, [initialized]);

    return (
        <div className="App">
            <h1>Home</h1>
            <Link to="/author">author</Link>
            <p>{quote.author}</p>
            <p>{quote.text}</p>
            <p>{quote.uuid}</p>
            <button onClick={() => fetchQuote()}>Click me</button>
        </div>
    );
};

export default Home;
