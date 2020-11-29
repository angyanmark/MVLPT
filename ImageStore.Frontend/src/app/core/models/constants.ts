export const Roles = {
  Admin: 'Admin',
  User: 'User',
};

export const allRoles = () => {
  return Object.keys(Roles).map((x) => Roles[x]);
};

export const FormAutosaveDelay = 2000;
