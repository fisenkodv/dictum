import './styles.scss';

import React from 'react';
import { Link } from 'react-router-dom';

const Home: React.FC = () => {
    return (
        <div className="App">
            <h1>Home</h1>
            <Link to="/author">author</Link>
        </div>
    );
};

export default Home;
