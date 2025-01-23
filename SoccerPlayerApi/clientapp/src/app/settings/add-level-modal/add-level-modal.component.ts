import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CreateLevelDto } from 'src/app/models/levels/createLevelDto';
import { ResponseDto } from 'src/app/models/responseDto';
import { LevelService } from 'src/app/services/level.service';

@Component({
  selector: 'app-level-modal',
  templateUrl: './add-level-modal.component.html',
  styleUrls: ['./add-level-modal.component.css']
})
export class LevelModalComponent {
  @Input() dimensionId: number | null = null;  // Accept dimensionId as input
  @Input() showModal: boolean = false;  // Control whether to show the modal
  @Output() levelAdded = new EventEmitter<CreateLevelDto>();  // Emit event when level is added
  newLevel: CreateLevelDto = { value: '', dimensionId: 0, ancestorId: null };

  constructor(private levelService: LevelService) { }

  ngOnInit() {
    if (this.dimensionId) {
      this.newLevel.dimensionId = this.dimensionId; 
    }
  }

  closeModal() {
    this.showModal = false;  // Close the modal
  }

  addLevel() {
    if (!this.dimensionId) return;

    this.newLevel.dimensionId = this.dimensionId; 

    this.levelService.createLevel(this.newLevel).subscribe({
      next: (response: ResponseDto<number>) => {
        this.levelAdded.emit(this.newLevel);
        this.closeModal();
      },
      error: (err) => {
        console.error('Error creating level:', err);
      }
    });
  }
}
