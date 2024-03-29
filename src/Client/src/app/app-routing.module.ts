import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountComponent } from './account/account.component';
import { ProfileComponent } from './account/profile/profile.component';
import { SettingsComponent } from './account/settings/settings.component';
import { AuthComponent } from './auth/auth.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { Roles } from './auth/roles';
import { CollectiblesComponent } from './collectibles/collectibles.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ElectronicsComponent } from './electronics/electronics.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ItemDetailsComponent } from './shared/items/item-details/item-details.component';
import { SneakersComponent } from './sneakers/sneakers.component';
import { StreetwearComponent } from './streetwear/streetwear.component';
import { AsksComponent } from './shared/asks/asks.component';
import { BidsComponent } from './shared/bids/bids.component';
import { UserBidsComponent } from './account/user-bids/user-bids.component';
import { UserAsksComponent } from './account/user-asks/user-asks.component';
import { FaqComponent } from './faq/faq.component';
import { AskFaqComponent } from './faq/ask-faq/ask-faq.component';
import { CustomerServiceFaqComponent } from './faq/customer-service-faq/customer-service-faq.component';
import { BidFaqComponent } from './faq/bid-faq/bid-faq.component';
import { SupervisorPanelComponent } from './supervisor-panel/supervisor-panel.component';
import { ManagementComponent } from './supervisor-panel/management/management.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { EmployeesComponent } from './admin-panel/employees/employees.component';
import { ProductsComponent } from './admin-panel/products/products.component';
import { AdminAndSupervisorSettingsComponent } from './shared/admin-and-supervisor-settings/admin-and-supervisor-settings.component';
import { TransactionsComponent } from './account/transactions/transactions.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent, pathMatch: 'full' },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  {
    path: 'account',
    component: AccountComponent,
    canActivate: [AuthGuard],
    data: { roles: [Roles.User] },
    children: [
      { path: '', redirectTo: 'profile', pathMatch: 'full' },
      { path: 'profile', component: ProfileComponent },
      {
        path: 'bids',
        component: UserBidsComponent
      },
      { path: 'asks', component: UserAsksComponent },
      { path: 'transactions', component: TransactionsComponent },
      { path: 'settings', component: SettingsComponent }
    ]
  },
  {
    path: 'supervisor-panel',
    component: SupervisorPanelComponent,
    canActivate: [AuthGuard],
    data: { roles: [Roles.Supervisor] },
    children: [
      { path: '', redirectTo: 'management', pathMatch: 'full' },
      { path: 'management', component: ManagementComponent },
      { path: 'settings', component: AdminAndSupervisorSettingsComponent }
    ]
  },
  {
    path: 'admin-panel',
    component: AdminPanelComponent,
    canActivate: [AuthGuard],
    data: { roles: [Roles.Admin] },
    children: [
      { path: '', redirectTo: 'employees', pathMatch: 'full' },
      { path: 'employees', component: EmployeesComponent },
      { path: 'products', component: ProductsComponent },
      { path: 'settings', component: AdminAndSupervisorSettingsComponent }
    ]
  },
  {
    path: 'items/:id',
    component: ItemDetailsComponent
  },
  {
    path: 'sneakers',
    component: SneakersComponent
  },
  {
    path: 'streetwear',
    component: StreetwearComponent
  },
  {
    path: 'electronics',
    component: ElectronicsComponent
  },
  {
    path: 'collectibles',
    component: CollectiblesComponent
  },
  {
    path: 'sell',
    component: AsksComponent
  },
  {
    path: 'buy',
    component: BidsComponent
  },
  {
    path: 'faq',
    component: FaqComponent
  },
  {
    path: 'faq/ask',
    component: AskFaqComponent
  },
  {
    path: 'faq/bid',
    component: BidFaqComponent
  },
  {
    path: 'faq/customer-service',
    component: CustomerServiceFaqComponent
  },

  { path: '**', pathMatch: 'full', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
