# Deploys the content of a directory to a Git branch using git worktree.
# Usage: ./gh-page-command.ps1 -SourceDir <path-to-dir> -Branch <branch-name>

param (
    [string]$SourceDir = "Builds/Web",
    [string]$Branch = "gh-pages",
    [string]$WorktreeDir = "gh-pages-worktree"
)

$ErrorActionPreference = 'Stop'

# 1. Resolve paths
$scriptPath = $MyInvocation.MyCommand.Path
$projectRoot = (Resolve-Path (Join-Path (Get-Item $scriptPath).Directory.FullName "..")).Path
$fullSourcePath = Join-Path $projectRoot $SourceDir
$fullWorktreePath = Join-Path $projectRoot $WorktreeDir

if (-not (Test-Path $fullSourcePath)) {
    Write-Error "Source directory not found at '$fullSourcePath'"
    exit 1
}

# 2. Cleanup previous worktree if it exists
if (Test-Path $fullWorktreePath) {
    Write-Host "Removing existing worktree directory..."
    # Use Remove-Item with -Recurse and -Force for robustness
    Remove-Item $fullWorktreePath -Recurse -Force
}
# Prune any lingering worktree info from .git
git worktree prune

# 3. Check if the target branch exists and create it if not
$branchExists = git branch --list --all | ForEach-Object { $_.Trim() } | Where-Object { $_ -eq $Branch -or $_ -eq "remotes/origin/$Branch" }
if (-not $branchExists) {
    Write-Host "Branch '$Branch' not found. Creating it as an orphan branch..."
    # Create an orphan branch without checking it out
    git switch --orphan $Branch
    git rm -rf .
    git commit --allow-empty -m "Initial commit for $Branch branch"
    # Go back to the previous branch
    git switch -
    Write-Host "Orphan branch '$Branch' created."
}

# 4. Add the worktree
Write-Host "Creating worktree at '$WorktreeDir' for branch '$Branch'..."
git worktree add $fullWorktreePath $Branch

try {
    # 5. Copy files to the worktree
    Write-Host "Cleaning worktree directory..."
    Get-ChildItem -Path $fullWorktreePath -Force | Where-Object { $_.Name -ne '.git' } | Remove-Item -Recurse -Force

    Write-Host "Copying build artifacts to worktree..."
    Copy-Item -Path (Join-Path $fullSourcePath "*") -Destination $fullWorktreePath -Recurse

    # 6. Commit and push from the worktree
    Write-Host "Committing and pushing from worktree..."
    # Use -C flag to run git commands in the context of the worktree's directory
    git -C $fullWorktreePath add .

    if (git -C $fullWorktreePath status --porcelain) {
        git -C $fullWorktreePath commit -m "Deploy content to $Branch branch"
        git -C $fullWorktreePath push -u origin $Branch --force
        Write-Host "Successfully deployed to $Branch."
    } else {
        Write-Host "No changes to deploy."
    }
}
finally {
    # 7. Cleanup the worktree
    Write-Host "Removing worktree..."
    git worktree remove $fullWorktreePath
    Write-Host "Deployment script finished."
}
