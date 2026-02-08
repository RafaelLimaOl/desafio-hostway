import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';
import { Car } from '../../models/car.model';
import { ParkingLot } from '../../models/parkingLot.model';
import { Space } from '../../models/space.model';

@Component({
  selector: 'app-operator-dash',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  templateUrl: './operator-dash.html',
  styleUrl: './operator-dash.css',
})
export class OperatorDash implements OnInit {
  cars: Car[] = [];
  loading = false;
  newCarForm!: FormGroup;

  spaces: Space[] = [];
  showSpaces = false;

  parkingLots: ParkingLot[] = [];
  loadingParkingLots = false;

  selectedCarId: string | null = null;
  selectedSpaceId: string | null = null;
  showCarSelect = false;

  userSessions: any[] = [];
  loadginSessions = false;
  sessionById: any = {};

  editCarForm!: FormGroup;
  editingCarId: string | null = null;
  isEditing = false;

  colors: string[] = [
    'Preto',
    'Branco',
    'Prata',
    'Cinza',
    'Vermelho',
    'Azul',
    'Verde',
    'Amarelo',
    'Marrom',
    'Laranja',
  ];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
  ) {}

  ngOnInit() {
    this.loadCars();
    this.loadSessions();
    this.loadParkingLots();

    this.newCarForm = this.fb.group({
      licensePlate: ['', Validators.required],
      color: ['', Validators.required],
      model: ['', Validators.required],
    });

    this.editCarForm = this.fb.group({
      licensePlate: ['', Validators.required],
      color: ['', Validators.required],
      model: ['', Validators.required],
      isActive: ['', Validators.required],
    });
  }

  loadCars() {
    this.loading = true;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .get<any>('https://localhost:7030/api/car', { headers })
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe({
        next: (response) => {
          this.cars = response.data;
        },
        error: (err) => {
          this.cars = [];
        },
      });
  }

  loadSessions() {
    this.loadginSessions = true;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .get<any>('https://localhost:7030/api/session', { headers })
      .pipe(
        finalize(() => {
          this.loadginSessions = false;
        }),
      )
      .subscribe({
        next: (response) => {
          this.userSessions = response.data;
        },
        error: (err) => {
          this.userSessions = [];
        },
      });
  }

  addCar() {
    if (this.newCarForm.invalid) return;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const body = {
      licensePlate: this.newCarForm.value.licensePlate,
      color: this.newCarForm.value.color,
      model: this.newCarForm.value.model,
      isActive: true,
    };

    this.http.post<any>('https://localhost:7030/api/car', body, { headers }).subscribe({
      next: (response) => {
        this.newCarForm.reset();
        this.loadCars();
      },
    });
  }

  editCar() {
    if (this.editCarForm.invalid || !this.editingCarId) return;
    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const body = {
      licensePlate: this.editCarForm.value.licensePlate,
      color: this.editCarForm.value.color,
      model: this.editCarForm.value.model,
      isActive: this.editCarForm.value.isActive,
    };

    this.http
      .put(`https://localhost:7030/api/car/${this.editingCarId}`, body, { headers })
      .subscribe({
        next: () => {
          this.editCarForm.reset();
          this.isEditing = false;
          this.loadCars();
        },
      });
  }

  onEdit(car: any) {
    this.editingCarId = car.id;
    this.isEditing = true;

    this.editCarForm.patchValue({
      licensePlate: car.licensePlate,
      color: car.color,
      model: car.model,
      isActive: car.isActive,
    });
  }

  onDelete(id: string) {
    const confirmDelete = confirm('Tem certeza que deseja deletar este carro?');

    if (!confirmDelete) return;
    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.loading = true;

    this.http
      .delete(`https://localhost:7030/api/car/${id}`, { headers })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: () => {
          this.loadCars();
        },
        error: (err) => {
          alert('Erro ao deletar o carro');
        },
      });
  }

  loadParkingLots() {
    this.loadingParkingLots = true;

    this.http.get<any>('https://localhost:7030/api/parking-lot/session').subscribe({
      next: (response) => {
        this.parkingLots = response.data;
        this.loadingParkingLots = false;
      },
      error: () => {
        alert('Erro ao buscar estacionamentos');
        this.loadingParkingLots = false;
      },
    });
  }

  viewSpaces(parkingLotId: string) {
    this.showSpaces = false;

    this.http.get<any>(`https://localhost:7030/api/space/space/${parkingLotId}`).subscribe({
      next: (response) => {
        this.spaces = response.data;
        this.showSpaces = true;
      },
      error: () => {
        alert('Erro ao buscar vagas');
      },
    });
  }

  startSession(spaceId: string) {
    this.selectedSpaceId = spaceId;
    this.showCarSelect = true;
  }

  createSession() {
    if (!this.selectedCarId || !this.selectedSpaceId) return;

    const body = {
      carId: this.selectedCarId,
      spaceId: this.selectedSpaceId,
      entryTime: new Date().toISOString(),
    };

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http.post('https://localhost:7030/api/session', body, { headers }).subscribe({
      next: () => {
        alert('Sessão iniciada!');
        this.showCarSelect = false;
        this.selectedCarId = null;
        this.loadSessions();
      },
      error: () => alert('Erro ao iniciar sessão'),
    });
  }

  closeSession(sessionId: string) {
    this.http.get<any>(`https://localhost:7030/api/session/${sessionId}`).subscribe({
      next: (response) => {
        this.sessionById = response.data;

        const confirmCloseSession = confirm(
          `Tem certeza que deseja fechar a sessão?\n` +
            `Tempo total: ${this.sessionById.totalTime}\n` +
            `Valor total: ${this.sessionById.amountChanged}`,
        );

        if (!confirmCloseSession) return;

        const token = localStorage.getItem('accessToken');

        const headers = new HttpHeaders({
          Authorization: `Bearer ${token}`,
        });

        this.http
          .put(`https://localhost:7030/api/session/close/${sessionId}`, {}, { headers })
          .subscribe({
            next: () => {
              this.loadSessions();
            },
            error: (err) => {
              console.error('Erro ao fechar sessão', err);
            },
          });
      },
      error: (err) => {
        console.error('Erro ao obter detalhes da sessão', err);
      },
    });
  }
}
