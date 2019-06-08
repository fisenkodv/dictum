import axios from 'axios';

import { Quote } from '../models';
import { baseUrl } from './constants';

export class QuotesApi {
    public static getRandomQuote(): Promise<Quote> {
        return axios
            .get(`${baseUrl}/dictum`)
            .then(response => response.data)
            .catch(error => console.error(error));
    }
}
