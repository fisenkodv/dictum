import { RouteConfig } from 'react-router-config';

import Home from './pages/home';
import Layout from './pages/layout';
import Author from './pages/author';

const routes: RouteConfig[] = [
    {
        component: Layout,
        routes: [
            {
                path: '/',
                exact: true,
                component: Home
            },
            {
                path: '/author',
                exact: true,
                component: Author
            }
            // {
            //     path: '/child/:id',
            //     component: Child,
            //     routes: [
            //         {
            //             path: '/child/:id/grand-child',
            //             component: GrandChild
            //         }
            //     ]
            // }
        ]
    }
];

export default routes;
