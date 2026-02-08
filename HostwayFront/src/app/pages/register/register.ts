import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css'],
})
export class Register {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
  ) {
    this.registerForm = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', Validators.required],
      deficiencia: [false],
      tipoUsuario: ['operator', Validators.required],
    });
  }

  onSubmit() {
    if (!this.registerForm.valid) return;

    const body = {
      email: this.registerForm.value.email,
      username: this.registerForm.value.nome,
      password: this.registerForm.value.senha,
      role: this.registerForm.value.tipoUsuario,
      haveDeficiency: this.registerForm.value.deficiencia,
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    this.http.post('https://localhost:7030/api/auth/register', body, { headers }).subscribe({
      next: () => {
        alert('Usuário registrado com sucesso!');
        this.registerForm.reset();
      },
      error: (err) => {
        alert('Erro ao registrar usuário.');
      },
    });
  }
}
