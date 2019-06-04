import React from 'react';
import { renderRoutes, RouteConfig } from 'react-router-config';

export const Layout: React.FC = ({ route }: RouteConfig) => (
    <div>
        <h1>Layout</h1>
        {/* child routes won't render without this */}
        {renderRoutes(route.routes)}
    </div>
);

export default Layout;
