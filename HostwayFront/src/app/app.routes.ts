import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Account } from './pages/account/account';
import { AdminDash } from './pages/admin-dash/admin-dash';
import { Hero } from './pages/hero/hero';
import { Login } from './pages/login/login';
import { OperatorDash } from './pages/operator-dash/operator-dash';
import { ParkingLot } from './pages/parking-lot/parking-lot';
import { Register } from './pages/register/register';

export const routes: Routes = [
  { path: '', component: Hero },
  { path: 'login', component: Login, pathMatch: 'full' },
  { path: 'register', component: Register },
  { path: 'dash-operator', component: OperatorDash },
  { path: 'dash-admin', component: AdminDash },
  { path: 'parking-lot', component: ParkingLot },
  { path: 'account', component: Account },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
