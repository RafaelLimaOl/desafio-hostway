import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ParkingLot } from '../../models/parkingLot.model';

@Component({
  selector: 'app-admin-dash',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './admin-dash.html',
  styleUrl: './admin-dash.css',
})
export class AdminDash implements OnInit {
  loading = false;
  parkingLot: ParkingLot[] = [];
  newParkingLotForm!: FormGroup;
  newSpaceForm!: FormGroup;

  ngOnInit() {
    this.loadParkingLot();

    this.newParkingLotForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      firstHourAmount: ['', Validators.required],
      extraHourAmount: ['', Validators.required],
    });

    this.newSpaceForm = this.fb.group({
      spaceNumber: ['', Validators.required],
      isAccessibleParkingSpace: ['', Validators.required],
      parkingLotId: ['', Validators.required],
    });
  }

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
  ) {}

  loadParkingLot() {
    this.loading = true;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .get<any>('https://localhost:7030/api/parking-lot', { headers })
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe({
        next: (response) => {
          this.parkingLot = response.data;
        },
        error: (err) => {
          this.parkingLot = [];
        },
      });
  }

  addParkingLot() {
    if (this.newParkingLotForm.invalid) return;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const body = {
      name: this.newParkingLotForm.value.name,
      address: this.newParkingLotForm.value.address,
      firstHourAmount: this.newParkingLotForm.value.firstHourAmount,
      extraHourAmount: this.newParkingLotForm.value.extraHourAmount,
      isActive: true,
    };

    this.http.post<any>('https://localhost:7030/api/parking-lot', body, { headers }).subscribe({
      next: (response) => {
        this.newParkingLotForm.reset();
        this.loadParkingLot();
      },
    });
  }

  onDelete(id: string) {
    const confirmDelete = confirm('Tem certeza que deseja deletar este estacionamento?');

    if (!confirmDelete) return;
    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.loading = true;

    this.http
      .delete(`https://localhost:7030/api/parking-lot/${id}`, { headers })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: () => {
          this.loadParkingLot();
        },
        error: (err) => {
          alert('Erro ao deletar o estacionamento');
        },
      });
  }

  addSpace() {
    if (this.newSpaceForm.invalid) return;

    const token = localStorage.getItem('accessToken');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const body = {
      spaceNumber: this.newSpaceForm.value.spaceNumber,
      isAccessibleParkingSpace: this.newSpaceForm.value.isAccessibleParkingSpace,
      parkingLotId: this.newSpaceForm.value.parkingLotId,
      isActive: true,
    };

    this.http.post<any>('https://localhost:7030/api/space', body, { headers }).subscribe({
      next: (response) => {
        this.newSpaceForm.reset();
        this.loadParkingLot();
      },
    });
  }
}
