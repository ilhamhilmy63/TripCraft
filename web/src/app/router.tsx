import { lazy, type ReactNode } from 'react';
import { createBrowserRouter, Navigate, type RouteObject } from 'react-router-dom';
import LoginPage from '@/auth/LoginPage';
import { ProtectedRoute } from '@/auth/ProtectedRoute';
import { RoleGuard } from '@/auth/RoleGuard';
import { LoadingSkeleton } from '@/shared/components/PageState';
import type { Role } from '@/shared/api/types';
import { AppLayout } from './AppLayout';

const NotFoundPage = lazy(() => import('./NotFoundPage'));
const TripsListPage = lazy(() => import('@/features/trips/TripsListPage'));
const TripDetailPage = lazy(() => import('@/features/trips/TripDetailPage'));
const AttractionsPage = lazy(() => import('@/features/trips/AttractionsPage'));

const MANAGER: Role[] = ['OperationsManager'];

const guard = (roles: Role[], element: ReactNode) => <RoleGuard roles={roles}>{element}</RoleGuard>;

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <Navigate to="/trips" replace />,
  },
  { path: '/login', element: <LoginPage /> },
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          { path: 'trips', element: guard(MANAGER, <TripsListPage />) },
          { path: 'trips/:id', element: guard(MANAGER, <TripDetailPage />) },
          { path: 'attractions', element: guard(MANAGER, <AttractionsPage />) },
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
