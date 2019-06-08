import React from 'react';
import { renderRoutes, RouteConfig } from 'react-router-config';

export const Layout: React.FC = ({ route }: RouteConfig) => <div>{renderRoutes(route.routes)}</div>;

export default Layout;
