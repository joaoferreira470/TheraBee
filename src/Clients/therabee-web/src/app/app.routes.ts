import { inject } from '@angular/core';
import { CanActivateFn, Routes, Router } from '@angular/router';

import { AuthPageComponent } from './pages/auth-page.component';
import { WorkspacePageComponent } from './pages/workspace-page.component';

const TOKEN_KEY = 'therabee_token';

const requireAuth: CanActivateFn = () => {
  const router = inject(Router);
  return localStorage.getItem(TOKEN_KEY) ? true : router.parseUrl('/login');
};

const redirectIfAuthenticated: CanActivateFn = () => {
  const router = inject(Router);
  return localStorage.getItem(TOKEN_KEY) ? router.parseUrl('/therapist') : true;
};

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
  {
    path: 'login',
    canActivate: [redirectIfAuthenticated],
    component: AuthPageComponent,
    data: { authMode: 'login' },
  },
  {
    path: 'register',
    canActivate: [redirectIfAuthenticated],
    component: AuthPageComponent,
    data: { authMode: 'register' },
  },
  {
    path: 'therapist',
    canActivate: [requireAuth],
    component: WorkspacePageComponent,
    data: { page: 'therapist' },
  },
  {
    path: 'patients/:patientId',
    canActivate: [requireAuth],
    component: WorkspacePageComponent,
    data: { page: 'patient' },
  },
  {
    path: 'patients/:patientId/sessions/:sessionId',
    canActivate: [requireAuth],
    component: WorkspacePageComponent,
    data: { page: 'session' },
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
