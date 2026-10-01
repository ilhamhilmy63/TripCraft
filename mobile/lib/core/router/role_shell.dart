import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import 'routes.dart';

class _Tab {
  const _Tab(this.path, this.label, this.icon);
  final String path;
  final String label;
  final IconData icon;
}

const _guideTabs = [
  _Tab(Routes.schedule, 'Schedule', Icons.event_note_outlined),
  _Tab(Routes.scan, 'Scan voucher', Icons.qr_code_scanner),
];

/// Bottom navigation for Student B's guide resource workflow.
class RoleShell extends StatelessWidget {
  const RoleShell({super.key, required this.location, required this.child});

  final String location;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    const tabs = _guideTabs;
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
