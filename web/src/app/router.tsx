import { lazy, type ReactNode } from 'react';
import { createBrowserRouter, Navigate, type RouteObject } from 'react-router-dom';
import LoginPage from '@/auth/LoginPage';
import { ProtectedRoute } from '@/auth/ProtectedRoute';
import { RoleGuard } from '@/auth/RoleGuard';
import { LoadingSkeleton } from '@/shared/components/PageState';
import type { Role } from '@/shared/api/types';
import { AppLayout } from './AppLayout';

const NotFoundPage = lazy(() => import('./NotFoundPage'));
const GuidesPage = lazy(() => import('@/features/resources/GuidesPage'));
const VehiclesPage = lazy(() => import('@/features/resources/VehiclesPage'));
const HotelsPage = lazy(() => import('@/features/resources/HotelsPage'));
const AvailabilityPage = lazy(() => import('@/features/resources/AvailabilityPage'));

const MANAGER: Role[] = ['OperationsManager'];

const guard = (roles: Role[], element: ReactNode) => <RoleGuard roles={roles}>{element}</RoleGuard>;

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <Navigate to="/resources/guides" replace />,
  },
  { path: '/login', element: <LoginPage /> },
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          { path: 'resources/guides', element: guard(MANAGER, <GuidesPage />) },
          { path: 'resources/vehicles', element: guard(MANAGER, <VehiclesPage />) },
          { path: 'resources/hotels', element: guard(MANAGER, <HotelsPage />) },
          { path: 'availability', element: guard(MANAGER, <AvailabilityPage />) },
          { path: '*', element: <NotFoundPage /> },
        ],
      },
    ],
  },
];

export const router = createBrowserRouter(routes, {
  future: {
    v7_relativeSplatPath: true,
    v7_fetcherPersist: true,
    v7_normalizeFormMethod: true,
    v7_partialHydration: true,
    v7_skipActionErrorRevalidation: true,
  },
});
