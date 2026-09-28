import type { NavItem } from '@/shared/components/Sidebar';

/** Student B navigation: guides, vehicles, hotels and availability. */
export const NAV_ITEMS: NavItem[] = [
  { to: '/resources/guides', label: 'Guides', roles: ['OperationsManager'] },
  { to: '/resources/vehicles', label: 'Vehicles', roles: ['OperationsManager'] },
  { to: '/resources/hotels', label: 'Hotels', roles: ['OperationsManager'] },
  { to: '/availability', label: 'Availability', roles: ['OperationsManager'] },
];
