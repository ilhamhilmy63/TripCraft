/// Every path in the app. Features navigate by these paths, so they never import each other's screens.
class Routes {
  const Routes._();

  static const splash = '/splash';
  static const login = '/login';
  static const register = '/register';
  static const notSupported = '/not-supported';

  // Guide
  static const schedule = '/schedule';
  static const tripDay = '/schedule/day';
  static const scan = '/scan';

  static const publicPaths = {login, register};
}

/// Landing page per role (PLAN.md section 2): staff use the React app, not this one.
String homeForRole(String role) => switch (role) {
  'Guide' => Routes.schedule,
  _ => Routes.notSupported,
};
