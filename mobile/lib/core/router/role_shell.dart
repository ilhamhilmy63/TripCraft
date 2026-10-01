import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import 'routes.dart';

class _Tab {
  const _Tab(this.path, this.label, this.icon);
  final String path;
  final String label;
  final IconData icon;
}

const _touristTabs = [
  _Tab(Routes.trips, 'My trips', Icons.luggage_outlined),
  _Tab(Routes.newTrip, 'New trip', Icons.add_circle_outline),
];

/// Bottom navigation for Student A's tourist trip-request screens.
class RoleShell extends StatelessWidget {
  const RoleShell({super.key, required this.location, required this.child});

  final String location;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    const tabs = _touristTabs;
    // The most specific tab whose path starts the location ("/trips/new" beats "/trips").
    final sorted = [...tabs]
      ..sort((a, b) => b.path.length.compareTo(a.path.length));
    final current = sorted.firstWhere(
      (t) => location.startsWith(t.path),
      orElse: () => tabs.first,
    );

    return Scaffold(
      body: child,
      bottomNavigationBar: NavigationBar(
        selectedIndex: tabs.indexOf(current),
        onDestinationSelected: (i) => context.go(tabs[i].path),
        destinations: [
          for (final t in tabs)
            NavigationDestination(icon: Icon(t.icon), label: t.label),
        ],
      ),
    );
  }
}
