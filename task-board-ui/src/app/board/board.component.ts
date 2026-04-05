import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../shared/services/auth.service';
import { BoardService } from '../shared/services/board.service';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './board.html',   
  styleUrls: ['./board.css']   
})
export class BoardComponent implements OnInit {

  activeTab: 'pending' | 'completed' = 'pending';
  tasks: any[] = [];

  loading = false;
  error = '';
  addError = ''; 
  showAddForm = true;

  newTitle = '';
  newDescription = '';
  adding = false;

  editingId = 0;
  editTitle = '';
  editDescription = '';

  userName = '';
  userEmail = '';
  showDeleteModal: boolean = false;
taskToDelete: any = null;
showCompleteModal: boolean = false;
taskToComplete: any = null;
checkboxEvent: any = null;

  constructor(
    private taskSvc: BoardService,  
    public authSvc: AuthService,
    private router: Router ,
    private cdr: ChangeDetectorRef 
  ) {}

  get userInitial() {
    return this.userName ? this.userName[0].toUpperCase() : '?';
  }

  get today() {
    return new Date().toLocaleDateString('en-IN', {
      weekday: 'long',
      day: 'numeric',
      month: 'long',
      year: 'numeric'
    });
  }

  ngOnInit() {
    this.authSvc.currentUser$.subscribe(u => {
      this.userName = u?.name ?? '';
      this.userEmail = u?.email ?? '';
    });

    this.loadTasks();
  }

  // GET /api/tasks
 loadTasks() {
  this.loading = true;

  this.taskSvc.getAll().subscribe({
    next: (res) => {
      console.log('API RESPONSE:', res);

      this.tasks = res.map((t: any) => {
  const fixed = t.createdAt.replace(' ', 'T') + 'Z';

  const utc = new Date(fixed);

  return {
    ...t,
    createdAt: new Date(
      utc.getTime() + (5.5 * 60 * 60 * 1000)
    )
  };
});
      this.loading = false;

      this.cdr.detectChanges(); 
    },
    error: (err) => {
      console.error(err);
      this.loading = false;

      this.cdr.detectChanges();
    }
  });
}


  get pendingTasks() {
     console.log('Filtering tasks:', this.tasks);
  return this.tasks.filter(t => t.status?.toLowerCase() === 'pending');
}

get completedTasks() {
  return this.tasks.filter(t => t.status?.toLowerCase() === 'completed');
}

  // POST /api/tasks
  addTask() {
    if (!this.newTitle.trim()) return;

    this.adding = true;
    this.addError = ''; 

    this.taskSvc.create({
      title: this.newTitle,
      description: this.newDescription
    }).subscribe({
      next: (task) => {
         this.newTitle = '';
          this.newDescription = '';
          this.adding = false;

          this.loadTasks(); // reload

          this.cdr.detectChanges();
      },
      error: () => {
        this.addError = 'Failed to add task';
        this.adding = false;
      }
    });
  }

  // EDIT
  startEdit(task: any) {
    this.editingId = task.id;
    this.editTitle = task.title;
    this.editDescription = task.description;
  }

  cancelEdit() {
    this.editingId = 0;
  }

  // PUT /api/tasks/{id}
  saveEdit(task: any) {
    this.taskSvc.update(task.id, {
      title: this.editTitle,
      description: this.editDescription
    }).subscribe({
      next: (updated) => {
        this.loadTasks();
        this.cancelEdit();
      },
      error: () => {
        this.error = 'Update failed';
      }
    });
  }

  // PUT /api/tasks/{id}/complete
  completeTask(task: any) {
    this.taskSvc.complete(task.id).subscribe({
      next: (updated) => {
    this.loadTasks();
      },
      error: () => {
        this.error = 'Failed to mark complete';
      }
    });
  }

  // DELETE /api/tasks/{id}
 deleteTask(task: any) {
  this.taskToDelete = task;
  this.showDeleteModal = true;
}

  formatDate(date: string) {
    return new Date(date).toLocaleString('en-IN');
  }

  logout() {
  this.authSvc.logout();       
  this.router.navigate(['/']);    
}
confirmDelete() {
  if (!this.taskToDelete) return;

  this.taskSvc.delete(this.taskToDelete.id).subscribe({
    next: () => {
      this.loadTasks();
      this.showDeleteModal = false;
      this.taskToDelete = null;
    },
    error: () => {
      this.error = 'Delete failed';
    }
  });
}

cancelDelete() {
  this.showDeleteModal = false;
  this.taskToDelete = null;
}

openCompleteModal(task: any, event: any) {
  this.taskToComplete = task;
  this.checkboxEvent = event; 
  this.showCompleteModal = true;
}

confirmComplete() {
  if (!this.taskToComplete) return;

  this.taskSvc.complete(this.taskToComplete.id).subscribe({
    next: () => {
      this.loadTasks();
      this.showCompleteModal = false;
      this.taskToComplete = null;
    },
    error: () => {
      this.error = 'Failed to mark complete';
    }
  });
}

cancelComplete() { 
  if (this.checkboxEvent) {
    this.checkboxEvent.target.checked = false;
  }

  this.showCompleteModal = false;
  this.taskToComplete = null;
}

}