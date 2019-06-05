import axios from 'axios';

import { Quote } from '../models';

export class QuotesApi {
    public static getRandomQuote(): Promise<Quote> {
        return axios
            .get('/dictum')
            .then(response => response.data)
            .catch(error => console.error(error));
    }
}
