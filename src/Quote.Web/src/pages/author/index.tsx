import './styles.scss';

import React from 'react';
import { Link } from 'react-router-dom';

export const Author: React.FC = () => {
    return (
        <div className="App">
            <h1>Author</h1>
            <Link to="/">home</Link>
        </div>
    );
};

export default Author;
