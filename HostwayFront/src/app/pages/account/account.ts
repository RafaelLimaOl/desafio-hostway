import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account implements OnInit {
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      nome: [''],
      email: [''],
      deficiencia: [false],
      role: [''],
      isActive: [''],
    });

    this.loadAccount();
  }

  loadAccount() {
    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http.get<any>('https://localhost:7030/api/account', { headers }).subscribe((response) => {
      const data = response.data;

      this.form.patchValue({
        nome: data.userName,
        email: data.email,
        deficiencia: data.haveDeficiency,
        role: data.role,
        isActive: data.isActive,
      });
    });
  }

  onSubmit() {
    const token = localStorage.getItem('accessToken');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    });

    const body = {
      username: this.form.value.nome,
      email: this.form.value.email,
      haveDeficiency: this.form.value.deficiencia,
      isActive: this.form.value.isActive,
    };

    this.http.put('https://localhost:7030/api/account', body, { headers }).subscribe({
      next: () => {
        alert('Dados atualizados com sucesso!');
      },
      error: () => {
        alert('Erro ao atualizar dados.');
      },
    });
  }

  logOut() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  deleteAccount() {
    const confirmDelete = confirm('Tem certeza que deseja deletar sua conta?');

    if (!confirmDelete) return;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http.delete('https://localhost:7030/api/account', { headers }).subscribe({
      next: () => {
        alert('Conta deletada com sucesso!');
        this.logOut();
      },
      error: () => {
        alert('Erro ao deletar conta.');
      },
    });
  }
}
