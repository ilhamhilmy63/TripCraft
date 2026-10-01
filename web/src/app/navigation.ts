import type { NavItem } from '@/shared/components/Sidebar';

/** Student A navigation: trip requests, itineraries and attractions. */
export const NAV_ITEMS: NavItem[] = [
  { to: '/trips', label: 'Trip requests', roles: ['OperationsManager'] },
  { to: '/attractions', label: 'Attractions', roles: ['OperationsManager'] },
];
