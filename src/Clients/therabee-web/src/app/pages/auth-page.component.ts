import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

type AuthMode = 'login' | 'register';

type AuthUser = {
  id: string;
  name: string;
  email: string;
  role: string;
};

type AuthForm = {
  name: string;
  profession: string;
  email: string;
  password: string;
};

const API_BASE_URL = 'http://localhost:6001';
const TOKEN_KEY = 'therabee_token';
const USER_KEY = 'therabee_user';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  templateUrl: './auth-page.component.html',
  styleUrl: './auth-page.component.css',
})
export class AuthPageComponent {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly routeData = toSignal(this.route.data, { initialValue: this.route.snapshot.data });

  readonly authMode = computed<AuthMode>(() => (this.routeData()?.['authMode'] as AuthMode) ?? 'login');
  readonly isBusy = signal(false);
  readonly apiMessage = signal('Prepara a tua sessao para aceder ao workspace.');

  readonly authForm = signal<AuthForm>({
    name: 'Joana Terapeuta',
    profession: 'Psicomotricista',
    email: 'demo@therabee.local',
    password: 'Password123!',
  });

  updateAuth(field: keyof AuthForm, value: string) {
    this.authForm.update((form) => ({ ...form, [field]: value }));
  }

  async authenticate() {
    const form = this.authForm();
    const mode = this.authMode();
    const route = mode === 'login' ? '/auth/login' : '/auth/register';
    const payload = mode === 'login'
      ? { email: form.email, password: form.password }
      : { name: form.name, email: form.email, password: form.password, profession: form.profession };

    this.isBusy.set(true);
    try {
      const response = await firstValueFrom(
        this.http.post<{ auth: { accessToken: string; user: AuthUser } }>(`${API_BASE_URL}${route}`, payload),
      );

      localStorage.setItem(TOKEN_KEY, response.auth.accessToken);
      localStorage.setItem(USER_KEY, JSON.stringify(response.auth.user));
      this.apiMessage.set(`Sessao iniciada como ${response.auth.user.name}.`);
      await this.router.navigate(['/therapist']);
    } catch (error) {
      console.error(error);
      this.apiMessage.set('Nao foi possivel autenticar. Confirma se a API esta a correr.');
    } finally {
      this.isBusy.set(false);
    }
  }

  async switchMode(mode: AuthMode) {
    await this.router.navigate([mode === 'login' ? '/login' : '/register']);
  }
}
