import { RouteConfig } from 'react-router-config';

import { Author, Home, Layout } from './pages';

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
