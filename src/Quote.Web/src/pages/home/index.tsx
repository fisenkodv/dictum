import './styles.scss';

import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { QuotesApi } from '../../services/api';
import { Quote } from '../../services/models';

export const Home: React.FC = () => {
    const [initialized, setInitialized] = useState(false);
    const [quote, setQuote] = useState<Quote>({ uuid: '', author: '', text: '' });

    useEffect(() => {
        if (!initialized) {
            setInitialized(true);
            fetchQuote();
        }
    });

    function fetchQuote() {
        return QuotesApi.getRandomQuote().then(x => setQuote(x));
    }

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
